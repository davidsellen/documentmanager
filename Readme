# Document API

The **Document API** is a web service for managing document uploads and metadata storage. It interacts with MongoDB for document metadata storage and MinIO for storing the actual document files (object storage).

## Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Prerequisites](#prerequisites)
- [Setup](#setup)
  - [Docker Setup](#docker-setup)
- [API Endpoints](#api-endpoints)
- [Environment Variables](#environment-variables)
- [Usage](#usage)
- [Running Tests](#running-tests)

## Features

- Upload and store documents
- Update existing documents
- Fetch document metadata by ID
- Store document metadata in MongoDB
- Store document files in MinIO (S3 compatible)

## Technologies

- **ASP.NET Core** (C#)
- **MongoDB** for metadata storage
- **MinIO** for object storage (S3 compatible)
- **Docker** and **Docker Compose** for container orchestration

## Prerequisites

Make sure you have the following installed:

- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Setup

### Docker Setup

1. Clone this repository:

   ```bash
   git clone https://github.com/davidsellen/documentmanager.git
   cd document-api
   ```

2. Ensure the `docker-compose.yml` file is properly configured for your environment. The default settings use MongoDB and MinIO locally.

3. Build and start the services:

   ```bash
   docker-compose up --build
   ```

   This will:
   - Build the `document-api` service.
   - Start MongoDB on port `27017`.
   - Start MinIO on ports `9000` (API) and `9001` (Console).

4. Access the application:

   - **Document API**: `http://localhost:5000`
   - **MinIO Console**: `http://localhost:9001` (Login: `minioadmin` / `minioadmin`)

   You can upload and manage documents via the API, and manage MinIO via its web console.

## API Endpoints

### Upload Document

- **POST** `/api/documents/upload`
- **Description**: Upload a new document and save its metadata.
- **Request**: Multipart form with a file.
- **Response**: Returns the created document metadata.

  ```bash
  curl -X POST http://localhost:5000/api/documents/upload \
       -F "file=@/path/to/your/document.pdf"
  ```

### Update Document

- **PUT** `/api/documents/{id}`
- **Description**: Update an existing document with a new file.
- **Request**: Multipart form with a file.
- **Response**: 200 OK on success.

  ```bash
  curl -X PUT http://localhost:5000/api/documents/123 \
       -F "file=@/path/to/your/new-document.pdf"
  ```

### Get Document by ID

- **GET** `/api/documents/{id}`
- **Description**: Retrieve document metadata by its ID.
- **Response**: Returns the document metadata.

  ```bash
  curl http://localhost:5000/api/documents/123
  ```

## Environment Variables

The following environment variables are used by the API and services:

| Variable                       | Description                           | Default Value                       |
|---------------------------------|---------------------------------------|-------------------------------------|
| `ASPNETCORE_ENVIRONMENT`        | ASP.NET Core environment              | Development                        |
| `MongoDb__ConnectionString`     | MongoDB connection string             | `mongodb://mongo:27017`            |
| `MongoDb__DatabaseName`         | MongoDB database name                 | `DocumentManagementDb`             |
| `Minio__Endpoint`               | MinIO endpoint (API URL)              | `minio:9000`                       |
| `Minio__AccessKey`              | MinIO access key                      | `minioadmin`                       |
| `Minio__SecretKey`              | MinIO secret key                      | `minioadmin`                       |

These can be customized in the `docker-compose.yml` file or set in your local environment if running without Docker.

## Usage

Once the application is running:

1. Use the provided API endpoints to upload, update, or fetch documents.
2. The actual files will be stored in MinIO, and metadata like filename, upload date, and file type will be saved in MongoDB.

You can access the MinIO console via `http://localhost:9001` and explore the object storage. The default credentials are:

- **Username**: `minioadmin`
- **Password**: `minioadmin`

## Running Tests

Unit tests for the API can be run using NUnit and Moq. Tests cover the following scenarios:

- Upload document success and failure
- Update document success and failure
- Fetch document by ID (existing and not found)

### Running Unit Tests

If you have the [.NET SDK](https://dotnet.microsoft.com/download) installed, run the tests with:

```bash
dotnet test
```

This will execute all tests in the project and provide output about test results.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
```
