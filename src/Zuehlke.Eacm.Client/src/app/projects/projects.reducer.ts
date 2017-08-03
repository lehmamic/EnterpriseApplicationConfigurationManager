import { Action } from "@ngrx/store";
import { ProjectsState, initialProjectState } from "./projects.state";
import { LOAD_PROJECTS, LOAD_PROJECTS_SUCCESS, Actions, LOAD_PROJECTS_FAILED } from "./project.actions";

export function projectsReducer(state: ProjectsState = initialProjectState, action: Actions) {
  switch (action.type) {
    case LOAD_PROJECTS:
      return { ...state, isLoadingProjects: true };

    case LOAD_PROJECTS_SUCCESS:
      return { ...state, projects: action.payload };

    default:
      return state;
  }
};
