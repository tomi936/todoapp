FROM python:2.7.16-slim
WORKDIR /app
EXPOSE 80

COPY ["Users/src/requirements.txt", "."]
RUN pip install --trusted-host pypi.python.org -r requirements.txt

COPY ["Users/src/app.py", "."]
CMD ["python", "app.py"]