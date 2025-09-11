# HikingQuests - MVP Project Plan

## Project Overview (MVP)
Build a minimal viable product (MVP) for HikingQuests, a web application that allows users to discover, create, and share hiking quests. 
The application will allow users to view a list of quests, see details for each quest, and create new quests.
Focus on TDD (Test Driven Development), domain modeling, API and a simple user interface.

**Tools and Technologies**: C#, ASP.NET Core, xUnit, Moq, HTML/CSS, React, TypeScript.

---

## Workflow
- Follow TDD principles for domain logic
- Use Git for version control with feature branches
	- Commit frequently with clear messages
	- Small and focused pull requests
	- Keep branches named in line with tasks in this document (e.g., feature/quest-log-add))
- Update project plan as needed

---

## Out of Scope for MVP
- User authentication and authorization
- Database integration
- Advanced search and filtering
- Image upload functionality
- Notes for quests
- Responsive design for mobile devices

---

## Features - Backend (MVP)
1. **Quest Listing**: Get a list of hiking quests with basic information (title, status). 
2. **Quest Details**: Get detailed information about a specific quest (title, description, status).
3. **Create Quest**: Allow users to create and submit new hiking quests.
4. **Quest Status**: Implement quest statuses (Planned -> InProgress -> Completed)
5. **Update Quest Status**: Allow users to start and complete quests.
6. **Edit Quest**: Allow users to edit quest title and description.
7. **API Endpoints**: Create RESTful API endpoints for quest listing, details, and creation.

## Features - Frontend (MVP)
1. **Quest List View**: Display a list of quests with titles and statuses.
2. **Quest Detail View**: Show detailed information about a selected quest.
3. **Create Quest Form**: Provide a form for users to create new quests.
4. **Update Quest Status**: Buttons to start and complete quests.
5. **Edit Quest**: Form to edit quest title and description.
6. **API Integration**: Connect the frontend with the backend API for data fetching and submission.
7. **Error Handling**: Implement basic error handling and user feedback for API interactions.
8. **Basic Styling**: Apply basic CSS for a clean and user-friendly interface.

---

## Future Enhancements (Post-MVP)
1. **Search and Filter**: Implement basic search and filtering options for quests (e.g. status or title).
2. **Add notes**: Allow users to add notes to quests.
3. **Persistence**: Integrate a database to persist quest data.
4. **User Authentication**: Implement user registration and login functionality.
5. **Responsive Design**: Make the application responsive for mobile and tablet devices.
6. **Image Upload**: Allow users to upload images for quests.

---

## Task Breakdown

### Domain (C#)
- [x] Set up ASP.NET Core project.
- [x] Set up xUnit and Moq for testing.
- [x] 'QuestItem' with validation and tests:
	- [x] Quest constructor.
	- [x] Start Quest (Planned -> InProgress)).
	- [x] Complete Quest (InProgress -> Completed).
- [x] 'QuestLog' with validation and tests.
	- [x] Add Quest.
	- [x] Get All Quests.
	- [x] Get Quest by ID/Title.

### Application (ASP.NET Core)
- [x] Set up ASP.NET Core Web API project
- [ ] 'QuestController' and tests:
	- [ ] GET '/quests' -> list all quests.
	- [ ] GET '/quests/{id}' -> get quest details by ID.
	- [ ] POST '/quests' -> create a new quest.
	- [ ] PATCH '/quests/{id}/start' -> start a quest.
	- [ ] PATCH '/quests/{id}/complete' -> complete a quest.
- [ ] In-memory data storage for quests (no database integration for MVP).
- [ ] Basic error handling: return correct HTTP status codes for invalid operations.

### UI (React + TypeScript)
- [x] Set up React project with TypeScript.
- [ ] Quest List View.
	- [ ] Render static list of quests.
	- [ ] Fetch quests from API.
	- [ ] Display status and title.
- [ ] Quest Detail View.
- [ ] Create Quest Form.
- [ ] Update Quest Status.
- [ ] Edit Quest Form.
- [ ] API Integration.
- [ ] Error Handling.
- [ ] Basic Styling.

---

## Project goals / Success Criteria

The MVP is considered **complete** when the following criteria are met:

1. **Domain Layer**
	- A 'QuestItem' class exists with methods to start and complete quests, along with appropriate validation and unit tests.
	- A 'QuestLog' class exists to manage a collection of quests, with methods to add and retrieve quests, along with appropriate validation and unit tests.

2. **API Layer**
	- A Working 'QuestController' exposes endpoints to:
		- List all quests (GET '/quests').
		- Retrieve a single quest by ID (GET '/quests/{id}').
		- Create a quest (POST '/quests').
		- Start a quest (PATCH '/quests/{id}/start').
		- Complete a quest (PATCH '/quests/{id}/complete').
	- Endpoints return appropriate HTTP status codes and error messages for invalid operations.

3. **Frontend Layer**
	- Quest List View displays a list of quests fetched from the API, showing titles and statuses.
	- Quest Detail View displays detailed information about a selected quest.
	- Users can create new quests via a form that submits data to the API.
	- Users can start and complete quests using buttons that trigger API calls.
	- Users can edit quest titles and descriptions via a form that submits updates to the API.
	- Basic error handling is implemented, providing user feedback for API interactions.
	- UI has basic styling for a clean and user-friendly interface.

4. **Workflow**
	- Code is developed following TDD principles, with tests written before implementation.
	- Git is used for version control with clear commit messages and small, focused pull requests.
	- Project plan is updated as needed to reflect changes in scope or requirements.
	- A short demo of viewing, creating, starting, and completing quests can be performed end-to-end.