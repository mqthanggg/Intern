FROM node:20.12.1-alpine

WORKDIR /app
COPY . /app
RUN npm install -g npm@10.5.0
RUN npm install -g @angular/cli@18.2.2
RUN npm install --force
CMD ["ng","serve","--host","0.0.0.0"]