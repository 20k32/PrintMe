FROM node:20-slim AS base
WORKDIR /app

FROM base AS build
WORKDIR /src
COPY package*.json ./
ENV NODE_ENV=development
RUN npm install --include=dev && npm install -g typescript vite
COPY tsconfig*.json vite.config.* ./

COPY . .
RUN vite build

FROM nginx:alpine AS final
COPY --from=build /src/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]