FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["ThirdPartOrderProcess/ThirdPartOrderProcess.csproj", "ThirdPartOrderProcess/"]
RUN dotnet restore "ThirdPartOrderProcess/ThirdPartOrderProcess.csproj"
COPY . .
WORKDIR "/src/ThirdPartOrderProcess"
RUN dotnet build "ThirdPartOrderProcess.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ThirdPartOrderProcess.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ThirdPartOrderProcess.dll"]