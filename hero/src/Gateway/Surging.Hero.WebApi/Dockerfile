FROM microsoft/dotnet:2.2.0-aspnetcore-runtime AS base
WORKDIR /app
ENV TZ=Asia/Shanghai 
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
ARG sln_name
COPY . .
RUN dotnet restore ${sln_name} && \
    dotnet build --no-restore -c Release ${sln_name}

FROM build AS publish
ARG host_workdir
WORKDIR ${host_workdir}
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
ARG host_name
ENV host_name=${host_name}
COPY --from=publish /app .
ENTRYPOINT dotnet ${host_name}