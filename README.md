# 🏥 CliniFlow - Backend API

API RESTful desarrollada en .NET 10 para la gestión integral de clínicas médicas. Este sistema permite la administración de pacientes, médicos, turnos y la generación de historias clínicas digitales.

## 🚀 Estado del Proyecto (v0.1.0 - Core Backend)
Actualmente, el backend cuenta con la arquitectura base, la capa de persistencia configurada con Entity Framework Core y los controladores principales operativos.

### ✅ Funcionalidades Implementadas
- **Gestión de Usuarios:** Sistema base de usuarios con roles (Admin, Profesional, Paciente).
- **Gestión de Pacientes:** CRUD completo de pacientes.
- **Gestión de Turnos (Appointments):** Creación y asignación de turnos médicos.
- **Historias Clínicas (Medical Records):**
  - Arquitectura optimizada y normalizada.
  - Relación 1 a 1 estricta con Turnos (`Appointment`).
  - Eliminación de redundancia de datos (desacoplamiento de Paciente/Profesional directo).

## 🛠️ Tecnologías
- **Framework:** .NET 10 (C#)
- **Base de Datos:** PostgreSQL
- **ORM:** Entity Framework Core
- **Documentación API:** Swagger / OpenAPI
- **Arquitectura:** Clean Architecture (Capas: Domain, Application, Infrastructure, API)

## ⚙️ Configuración y Ejecución

### Requisitos previos
- .NET SDK 10.0
- PostgreSQL instalado y corriendo.

### Pasos para levantar el proyecto
1. **Clonar el repositorio:**
   ```bash
   git clone <URL_DEL_REPO>