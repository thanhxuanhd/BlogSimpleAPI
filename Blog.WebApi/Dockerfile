#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-nanoserver-1809 AS build
WORKDIR /src
COPY ["Blog.WebApi/Blog.WebApi.csproj", "Blog.WebApi/"]
COPY ["Blog.Core/Blog.Core.csproj", "Blog.Core/"]
COPY ["Blog.Infrastructure/Blog.Infrastructure.csproj", "Blog.Infrastructure/"]
COPY ["Blog.Service/Blog.Service.csproj", "Blog.Service/"]
RUN dotnet restore "Blog.WebApi/Blog.WebApi.csproj"
COPY . .
WORKDIR "/src/Blog.WebApi"
RUN dotnet build "Blog.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Blog.WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Blog.WebApi.dll"]