FROM node:21-alpine3.18 as build

WORKDIR /app

COPY package.json package-lock.json angular.json /app

RUN npm install

COPY . .

EXPOSE 4200

CMD ["npx", "ng", "serve", "--host", "0.0.0.0"]
