import { Component, OnInit } from '@angular/core';
import { Store } from "@ngrx/store";

import { RootState } from "../../app.state";
import { Observable } from "rxjs/Observable";
import { Project } from "../projects.service";
import { LoadProjectsAction } from "../project.actions";

@Component({
  selector: 'app-all-projects',
  templateUrl: './all-projects.component.html',
  styleUrls: ['./all-projects.component.scss']
})
export class AllProjectsComponent implements OnInit {
  projects: Observable<Array<Project>>;

  constructor(private store: Store<RootState>) {
    this.projects = this.store.select(s => s.projects.projects);

    this.store.dispatch(new LoadProjectsAction());
  }

  ngOnInit() {
  }

}
