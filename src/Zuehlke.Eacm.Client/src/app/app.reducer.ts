import { ActionReducerMap, Action } from '@ngrx/store';
import { routerReducer } from '@ngrx/router-store';

import { RootState, AppState, initialAppState } from './app.state';

export function appReducer(state = initialAppState, action: Action): AppState {
  switch (action.type) {

    default: {
      return state;
    }
  }
};

export const reducers: ActionReducerMap<RootState> = {
  app: appReducer,
  routerReducer: routerReducer
};
