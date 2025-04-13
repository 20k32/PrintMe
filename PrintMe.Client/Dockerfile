FROM node:20 AS base
WORKDIR /app

FROM base AS build
WORKDIR /src
COPY package*.json ./
ENV NODE_ENV=development
RUN npm ci && npm install -g typescript
COPY tsconfig*.json ./
#RUN npm install --include=dev

COPY . .
RUN npx vite build

FROM nginx:alpine AS final
COPY --from=build /src/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]