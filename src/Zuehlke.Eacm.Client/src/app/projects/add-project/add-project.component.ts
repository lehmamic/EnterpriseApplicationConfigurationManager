import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Store } from "@ngrx/store";

import { RootState } from "../../app.state";
import { Project } from "../projects.service";
import { CreateProjectAction } from "../projects.actions";

@Component({
  selector: 'app-add-project',
  templateUrl: './add-project.component.html',
  styleUrls: ['./add-project.component.scss']
})
export class AddProjectComponent implements OnDestroy {
  projectForm: FormGroup

  constructor(private formBuilder: FormBuilder, private store: Store<RootState>) {
    this.projectForm = this.formBuilder.group({
      name: ['', Validators.required],
      description: '',
    });
  }

  public createProject() {
    var project: Project = {
      name: this.projectForm.controls['name'].value,
      description: this.projectForm.controls['description'].value
    };

    this.store.dispatch(new CreateProjectAction(project))
  }

  ngOnDestroy(): void {
  }
}
