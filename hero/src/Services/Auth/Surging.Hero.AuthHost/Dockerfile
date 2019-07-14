FROM microsoft/dotnet:2.2.0-runtime AS base
WORKDIR /app
ARG rpc_port=100
ARG http_port=8080
ARG ws_port=96
ENV TZ=Asia/Shanghai 
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone 
EXPOSE ${rpc_port} ${http_port} ${ws_port}

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY . .
ARG sln_name
RUN dotnet restore ${sln_name} && \
    dotnet build --no-restore -c Release ${sln_name}

FROM build AS publish
ARG host_workdir
WORKDIR ${host_workdir}
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
ARG host_name
ENV host_name=${host_name}
COPY --from=publish /app .
ENTRYPOINT dotnet ${host_name}