FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.14

RUN apk update \
    && apk add --no-cache icu-libs \
    && apk add --no-cache --virtual .build-deps \
        curl \
        bash \
        openssl \
        build-base \
    && rm -rf /var/cache/apk/*

# Create a working directory
WORKDIR /app

# expose port
EXPOSE 443

# set timezone
ENV TZ=Europe/Amsterdam
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

ENTRYPOINT ["/bin/bash", "-c" , "dotnet restore && dotnet dev-certs https && dotnet watch run -nowarn"]