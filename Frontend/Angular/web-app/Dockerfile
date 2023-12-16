# Build Stage
FROM node:latest AS build
WORKDIR /Source
COPY . .
RUN npm install --force
RUN npm run build --prod

# Serve Stage
FROM nginx:alpine
COPY --from=build /Source/dist/web-app /usr/share/nginx/html
EXPOSE 80 443
