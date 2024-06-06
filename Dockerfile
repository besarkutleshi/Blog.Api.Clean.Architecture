FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Blog.Presentation/Blog.Presentation.csproj", "Blog.Presentation/"]
COPY ["Blog.Application/Blog.Application.csproj", "Blog.Application/"]
COPY ["Blog.Domain/Blog.Domain.csproj", "Blog.Domain/"]
COPY ["Blog.SharedResources/Blog.SharedResources.csproj", "Blog.SharedResources/"]
COPY ["Blog.Infrastructure/Blog.Infrastructure.csproj", "Blog.Infrastructure/"]
RUN dotnet restore "./Blog.Presentation/Blog.Presentation.csproj"
COPY . .
WORKDIR "/src/Blog.Presentation"
RUN dotnet build "./Blog.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Blog.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.Presentation.dll"]