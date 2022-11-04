FROM python:3.10

WORKDIR /payment
ENV PRODUCTION_ENV=rabbitmq-container

COPY requirements.txt requirements.txt

RUN pip install -r requirements.txt

COPY . .

CMD ["python", "-u","card_checker.py"]