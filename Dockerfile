FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

RUN apt-get update && \
    apt-get install -y git clang gcc zlib1g-dev && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app

RUN git init && git branch -M main

RUN git config --global user.email "john.doe@mail.com" && \
    git config --global user.name "John Doe"

RUN git remote add origin https://github.com/SimonNyvall/B-branch.git

RUN git fetch origin main

RUN echo "# Placeholder" > README.md && \
    git add README.md && \
    git commit -m "Initial commit"

RUN git checkout -b test/branch1 && \
    git checkout -b test/branch2 && \
    git checkout -b test/branch3

COPY *.sln ./
COPY src/ ./src/
COPY tests/ ./tests/

RUN dotnet restore

RUN dotnet build --no-restore -c Debug

WORKDIR /app/src/CLI
RUN dotnet publish -r linux-x64 -c Release -o /app/publish

WORKDIR /app/tests/IntegrationTests

ENTRYPOINT ["dotnet", "test", "--no-build", "-v", "n"]