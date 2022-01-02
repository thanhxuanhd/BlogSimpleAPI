#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Blog.WebApi/Blog.WebApi.csproj", "Blog.WebApi/"]
COPY ["Blog.Core/Blog.Core.csproj", "Blog.Core/"]
COPY ["Blog.Infrastructure/Blog.Infrastructure.csproj", "Blog.Infrastructure/"]
COPY ["Blog.Service/Blog.Service.csproj", "Blog.Service/"]
RUN dotnet restore "Blog.WebApi/Blog.WebApi.csproj"
COPY . .
WORKDIR "/src/Blog.WebApi"
RUN dotnet build "Blog.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blog.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.WebApi.dll"]