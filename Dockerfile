# Этап 1: Сборка (build)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем только файлы проекта для восстановления зависимостей
COPY ["VirtualizationLab.csproj", "."]
RUN dotnet restore "VirtualizationLab.csproj"

# Копируем всё остальное и собираем
COPY . .
RUN dotnet build "VirtualizationLab.csproj" -c Release -o /app/build

# Этап 2: Публикация (publish)
FROM build AS publish
RUN dotnet publish "VirtualizationLab.csproj" -c Release -o /app/publish

# Этап 3: Финальный образ (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Указываем, что приложение слушает порт 80
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "VirtualizationLab.dll"]