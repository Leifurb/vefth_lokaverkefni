# Write your code here :-)
import sys


import time
import board
import busio
import rotaryio
import adafruit_tcs34725

from math import cos, sin, pi, sqrt, asin, exp
from pwmio import PWMOut
from digitalio import DigitalInOut, Direction, Pull
from adafruit_motor import motor as Motor

MAX_THROTTLE = 1
DEVELOPMENT = True

class Colors:
    GREEN = 3
    RED = 2
    BLUE = 1
    TABLE = 0

    def to_string(color):
        if color == 0:
            return "TABLE"
        if color == 1:
            return "BLUE"
        if color == 2:
            return "RED"
        if color == 3:
            return "GREEN"

class Vector:
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z

    def get_distance(self, other):
        return pow(self.x - other.x, 2) + pow(self.y - other.y, 2) + pow(self.z - other.z, 2)


class RGBSensor:
    def __init__(self):
        i2c = board.I2C()
        self.sensor = adafruit_tcs34725.TCS34725(i2c)
        self.sensor.integration_time = 2.4
        self.sensor.gain = 60
        self.colors = [Vector(255, 255, 180), Vector(9, 68, 78), Vector(255, 14, 8), Vector(5, 60, 13)]

    def get_color(self):
        rgb = self.sensor.color_rgb_bytes
        color = Vector(rgb[0], rgb[1], rgb[2])
        distances = [color.get_distance(i) for i in self.colors]
        return (distances.index(min(distances)))

class MotorController:
    def __init__(self, forward, backward, pin1, pin2):
        self.forward = PWMOut(forward, frequency=50)
        self.backward = PWMOut(backward, frequency=50)
        self.motor = Motor.DCMotor(self.forward, self.backward)
        self.motor.throttle = None
        self.enc = rotaryio.IncrementalEncoder(pin1, pin2)
        self.last_position = 0

    def get_pos(self):
        return self.enc.position;

    def set_throttle(self, throttle):
        self.motor.throttle = throttle * MAX_THROTTLE

class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def distance_to(self, other):
        return sqrt(pow(self.x - other.x, 2) + pow(self.x - other.x, 2))


class Car:
    def __init__(self, left_motor, right_motor):
        self.left_motor = left_motor
        self.right_motor = right_motor

        self.rgb_sensor = RGBSensor()

        self.history = self.rgb_sensor.get_color()
        self.last_position = [self.left_motor.get_pos(), self.right_motor.get_pos()]

        self.first_adjust = True

        self.left_motor.set_throttle(1)
        self.right_motor.set_throttle(1)

    def adjust(self, left, right):

        #print("Adjusting")

        car.left_motor.set_throttle(0)
        car.right_motor.set_throttle(0)

        left_done = False
        right_done = False

        target_left = self.left_motor.get_pos() + left
        target_right = self.right_motor.get_pos() + right

        if left < 0:
            self.left_motor.motor.throttle = -0.25
        elif left > 0:
            self.left_motor.motor.throttle = 0.25
        else:
            left_done = True

        if right < 0:
            self.right_motor.motor.throttle = -0.25
        elif right > 0:
            self.right_motor.motor.throttle = 0.25
        else:
            right_done = True


        while not right_done or not left_done:
            if not left_done:
                if left < 0:
                    if self.left_motor.get_pos() <= target_left:
                        left_done = True
                        self.left_motor.set_throttle(0)
                else:
                    if self.left_motor.get_pos() >= target_left:
                        self.left_motor.set_throttle(0)
                        left_done = True
            if not right_done:
                if right < 0:
                    if self.right_motor.get_pos() <= target_right:
                        self.right_motor.set_throttle(0)
                        right_done = True
                else:
                    if self.right_motor.get_pos() >= target_right:
                        self.right_motor.set_throttle(0)
                        right_done = True

        #print("Done adjusting")

    def get_distance(self):
        return abs(self.left_motor.get_pos() - self.last_position[0]) / 2 + abs(self.right_motor.get_pos() - self.last_position[1]) / 2

    def turn(self, last_color):
        #print("Start turning")


        #print("Back to RED")
        self.right_motor.motor.throttle = -0.25
        self.left_motor.motor.throttle = -0.25
        distance = 0
        found = False
        while True:
            color = self.rgb_sensor.get_color()
            if not found and (color == Colors.BLUE or color == Colors.RED):
                found = True
                self.history = color
                self.last_position[0] = self.left_motor.get_pos()
                self.last_position[1] = self.right_motor.get_pos()
            elif color == Colors.GREEN:
                distance = self.get_distance()
                break
                
        target = self.left_motor.get_pos() - distance * 1 / 4
            
        while self.left_motor.get_pos() > target:
            pass
            


        #print("We are on RED")
        #print(f"Distance between blue and red : {distance}")
        #print("Now rotate")

        #print(distance)

        target = asin(90 / distance) * 925
        
        print(Colors.to_string(last_color))

        if last_color == Colors.BLUE:
            self.adjust(target/2, -target/2)
        if last_color == Colors.RED:
            self.adjust(-target/2, target/2)

        #print("Full throttle")

        self.left_motor.set_throttle(1)
        self.right_motor.set_throttle(1)


    def update(self):
        color = self.rgb_sensor.get_color()
        

        if self.history == color:
            if color == Colors.BLUE:
                distance = self.get_distance()
                if distance > 1000:

                    self.right_motor.set_throttle(0)
                    self.left_motor.motor.throttle = 0.25

                    start = self.left_motor.get_pos()
                    while self.rgb_sensor.get_color() != Colors.GREEN:
                        pass

                    self.left_motor.set_throttle(0)

                    curve = self.left_motor.get_pos() - start

                    y = 0.751607 * curve + 35.3915

                    self.adjust(0, y)
                    self.left_motor.set_throttle(1)
                    self.right_motor.set_throttle(1)
            elif color == Colors.RED:
                distance = self.get_distance()
                if distance > 1000:

                    self.left_motor.set_throttle(0)
                    self.right_motor.motor.throttle = 0.25

                    start = self.right_motor.get_pos()
                    while self.rgb_sensor.get_color() != Colors.GREEN:
                        pass


                    self.right_motor.set_throttle(0)

                    curve = self.right_motor.get_pos() - start
                    
                    y = 0.751607 * curve + 35.3915

                    self.adjust(y, 0)
                    self.right_motor.set_throttle(1)
                    self.left_motor.set_throttle(1)
            return

        if color == Colors.TABLE:
            print(self.get_distance())
            last_color = self.history
            self.history = color
            self.turn(last_color)
            self.left_motor.set_throttle(1)
            self.right_motor.set_throttle(1)
            return

        #print(f"New color detected: {Colors.to_string(color)}")
        #print("Full throttle")

        self.left_motor.set_throttle(1)
        self.right_motor.set_throttle(1)



        self.history = color

        self.last_position[0] = self.left_motor.get_pos()
        self.last_position[1] = self.right_motor.get_pos()


left_motor = MotorController(board.D10, board.D9, board.D6, board.D7)
right_motor = MotorController(board.D11, board.D12, board.D5, board.D4)

car = Car(left_motor, right_motor)

while True:
    car.update()


