import { ProjectsState } from './projects/projects.state';
import { PAGE_TITLE } from './app.constants';
import { RouterReducerState } from "@ngrx/router-store/router-store";

export const initialAppState: AppState = {
  pageTitle: PAGE_TITLE
};

export interface AppState {
  pageTitle: string;
};

export interface RootState {
  app: AppState;
  routerReducer: RouterReducerState;
};
