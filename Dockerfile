# Используем официальный образ .NET
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем файлы проекта и восстанавливаем зависимости
COPY *.csproj .
RUN dotnet restore

# Копируем всё и собираем приложение
COPY . .
RUN dotnet publish -c release -o /app

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "VirtualizationLab.dll"]