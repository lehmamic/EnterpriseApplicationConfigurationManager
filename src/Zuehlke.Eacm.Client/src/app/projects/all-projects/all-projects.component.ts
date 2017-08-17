import { Component } from '@angular/core';
import { Store } from "@ngrx/store";

import { RootState } from "../../app.state";
import { Observable } from "rxjs/Observable";
import { Project } from "../projects.service";
import { LoadProjectsAction } from "../projects.actions";

@Component({
  selector: 'app-all-projects',
  templateUrl: './all-projects.component.html',
  styleUrls: ['./all-projects.component.scss']
})
export class AllProjectsComponent {
  projects: Observable<Array<Project>>;

  constructor(private store: Store<RootState>) {
    this.projects = this.store.select(s => s.projects.projects);

    this.store.dispatch(new LoadProjectsAction());
  }
}
