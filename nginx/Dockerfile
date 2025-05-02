FROM nginx:latest

COPY nginx.conf /etc/nginx/nginx.conf

COPY --from=build /app/frontend/coderepo/dist/coderepo /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
