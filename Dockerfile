FROM gcr.io/google-appengine/aspnetcore:2.2
#COPY . /app
ADD ./ /app
ENV ASPNETCORE_URLS=https://*:${PORT}
WORKDIR /app
ENTRYPOINT ["dotnet", "PlayTogether.dll"]	
