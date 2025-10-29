# HikingQuests - Deployment Project Plan

## Project Overview (Deployment)
HikingQuests is a web application that allows users to discover, create, and share hiking quests. 
The application allows users to view a list of quests, see details for each quest, create new quests and update/delete a quest.
As with the MVP, the focus remains on TDD (Test Driven Development), domain modeling, and a clean API/UI, 
but now extended with persistent data, authentication, CI/CD and deployment readiness.

---

## Tools and Technologies:

**Backend**:
C#, ASP.NET Core, xUnit, Moq

**Database**:
EF Core, PostgreSQL (Prod) / SQLite(local)

**Frontend**:
React, TypeScript, HTML/CSS, Vitest/Jest

**Deployment stack**:
Docker, GitHub Actions, Render / Fly.io (backend), Netlify / Vercel (frontend), Neon.tech / Supabase (database)

---

## Workflow
- Follow TDD principles for backend and frontend.
- Maintain minimum 80% test coverage across backend and frontend.
- Run tests automatically on each pull request.
- Use Git feature branches: small, frequent commits with clear messages.
- Do integration and end-to-end testing before deployment.
- Add secure database communication (SSL enforced).
- Use Google OAuth for sign-in to avoid handling sensitive credentials.
- Apply security measures (rate limiting, CORS, HTTPS, environment secrets).
- Store secrets in Github Actions secrets (no plaintext .env in repo).
- Deploy via containerized builds using Docker and Github Actions.
- Maintain clear environment separation: **Development / Staging / Production**.
- Document environment variables and secrets in README.md.
- Verify deployments through manual smoke tests.
- Apply migrations automatically during deployment.
- Schedule automated daily backups of production database.

---

## Existing Features - Backend (Implemented as part of MVP)
*1. **Quest Listing**: Get a list of hiking quests with basic information (title, status). *
*2. **Quest Details**: Get detailed information about a specific quest (title, description, status).
*3. **Create Quest**: Allow users to create and submit new hiking quests.*
*4. **Quest Status**: Implement quest statuses (Planned -> InProgress -> Completed).*
*5. **Update Quest Status**: Allow users to start and complete quests.*
*6. **Edit Quest**: Allow users to edit quest title and description.*
*7. **API Endpoints**: Create RESTful API endpoints for quest listing, details, and creation.*

## Existing Features - Frontend (Implemented as part of MVP)
*1. **Quest List View**: Display a list of quests with titles and statuses.*
*2. **Quest Detail View**: Show detailed information about a selected quest.*
*3. **Create Quest Form**: Provide a form for users to create new quests.*
*4. **Update Quest Status**: Buttons to start and complete quests.*
*5. **Edit Quest**: Form to edit quest title and description.*
*6. **API Integration**: Connect the frontend with the backend API for data fetching and submission.*
*7. **Error Handling**: Implement basic error handling and user feedback for API interactions.*
*8. **Basic Styling**: Apply basic CSS for a clean and user-friendly interface.*

## Planned Features - Backend (Deployment)

1. **Database integration**
	- Add EF Core with repository pattern.
	- Use SQLite for development.
	- Use PostgreSQL for production.
	- Apply migrations using 'dotnet ef database update' or automatic 'DBContext.Database.Migrate()' on startup.
2. **User Authentication**
	- Implement Google OAuth login
	- Store only Google 'sub' (user ID) and display name.
3. **Deployment Setup**
	- Create Dockerfile and docker-compose configuration.
	- Add rate limiting and CORS configuration.
	- Integrate with free PostgreSQL hosting (e.g. Neon / Supabase)
4. **CI/CD**
	- Add Github Actions for build, test, and deploy.
	- Run migrations automatically on deploy.

## Planned Features - Frontend (Deployment)
1. **Responsive Design**
	- For mobile and tablet devices.
2. **Search and Filter**
	- For quests (status/title/keyword).
3. **User Authentication Flow**
	- Google sign-in integration.
	- Authenticated API requests using access tokens from backend.
	- Display/edit local username (not tied to Google account).
4. **Build and Deployment**
	- Add environment configuration for API URL.
	- Dockerize frontend.
	- Prefer Netlify (simpler static deploy) or Vercel (automatic build from GitHub).

---

## Future Enhancements (Post-Deployment)

1. **Add experience points**
	- Integrate a "hiking experience points" counter.
	- Add experience gain when completing quests.
2. **Add notes**
	- Allow users to add notes to quests.

---

## Task Breakdown

### Domain (C#)
- [x] Add EF Core models for quests (QuestEntity).
- [] Add EF Core models for Users(UserEntity).
- [x] Configure domain-data mapping.
- [x] Add seed data for local testing.
- [x] Add persistence tests using SQLite provider.

### Application (ASP.NET Core)
- [x] Implement AddDbContext with EF Core.
- [x] Implement repository abstraction for quests (IQuestRepository)
- [] Implement repository abstraction for users (IUserRepository).
- [] Add AuthController for Google OAuth.
- [] Apply database migrations and test schema.
- [x] Add integration tests for CRUD (quests)
- [] Add integration tests for CRUD (users)
- [] Add integration tests for auth.
- [] Add rate limiting, CORS, and HTTPS redirection.
- [] Create Dockerfile and docker-compose setup for backend + DB.

### UI (React + TypeScript)
- [] Add search field and filtering logic.
- [] Add sorting button (title or status).
- [] Add sign-in and sign-out components.
- [] Add "Edit username" view for local display name.
- [] Integrate Google sign-in flow with backend OAuth.
- [] Add environment configuration for API base URL.
- [] Add responsive layout (CSS grid or Tailwind).
- [] Add basic Vitest/Jest UI tests for auth and CRUD flows.
- [] Dockerize frontend for deployment.

---

## Project goals / Success Criteria

The Deployment is considered **complete** when the following criteria are met:

1. **Domain Layer**
	- Domain models map correctly to EF Core entities.
	- Validation and transitions tested with EF InMemory provider.
	- Migrations verified on local and staging environments.

2. **API Layer**
	- Fully functional persistence using PostgreSQL.
	- AuthController successfully authenticates via Google OAuth.
	- CRUD endpoints secure, rate-limited, and integration-tested.
	- Docker containers (API + DB) run locally and deploy remotely.

3. **Frontend Layer**
	- Responsive UI.
	- Working authentication flow.
	- CRUD, search and sorting fully functional.
	- Integration with backend tested and verified post-deploy.

4. **Workflow**
	- TDD principles applied end-to-end.
	- Github Actions automates **Test -> Build -> Deploy**
	- Database migrations and smoke tests run automatically on deployment.
	- README documents environment variables and deployment steps.
	- Changelog updated per release.
	- All deployed services (API, DB, UI) verified healthy via smoke tests.