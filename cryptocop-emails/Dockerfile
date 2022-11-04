FROM python:3.10

WORKDIR /email

WORKDIR /email
ENV PRODUCTION_ENV=rabbitmq-container

COPY requirements.txt requirements.txt

RUN pip install -r requirements.txt

COPY . .

CMD ["python", "./email_service.py"]