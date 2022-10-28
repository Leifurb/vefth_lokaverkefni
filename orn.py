
from msilib.schema import Error


def dataLoad(filename):

    file = open(filename, 'r')
    data = []
    counter = 0
    for line in file:
        l = line.strip().split(' ')
        l[0] = int(l[0])
        l[1] = float(l[1])
        l[2] = int(l[2])

        if l[2] < 1 or l[2] > 4:
            print(f'Error line:{counter} values {l}: Must be a valid Bacteria')
            continue
        if l[1] < 0:
            print(f'Error line:{counter} values {l}: Growth rate must be positive ')
            continue
        if l[0] < 10 or l[0] > 100: 
            print(f'Error line:{counter} values {l}: Tempature must be beetween 10 and 100')
            continue
        counter += 1
        data.append(l)
    return data

a = dataLoad('filename.txt')
print(a)


