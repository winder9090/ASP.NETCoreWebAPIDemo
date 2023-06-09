#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ASP.NETCoreWebAPIDemo/ASP.NETCoreWebAPIDemo.csproj", "ASP.NETCoreWebAPIDemo/"]
COPY ["Service/Service.csproj", "Service/"]
COPY ["Model/Model.csproj", "Model/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Tasks/Tasks.csproj", "Tasks/"]
RUN dotnet restore "ASP.NETCoreWebAPIDemo/ASP.NETCoreWebAPIDemo.csproj"
COPY . .
WORKDIR "/src/ASP.NETCoreWebAPIDemo"
RUN dotnet build "ASP.NETCoreWebAPIDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ASP.NETCoreWebAPIDemo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASP.NETCoreWebAPIDemo.dll"]