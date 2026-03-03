import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CardComponent } from 'src/app/theme/shared/components/card/card.component';
import { debounceTime, distinctUntilChanged, map, Observable, OperatorFunction } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { from } from "linq-to-typescript"


@Component({
  selector: 'app-ibantools-page',
  imports: [CommonModule, CardComponent, FormsModule, NgbTypeaheadModule],
  templateUrl: './ibantools-page.component.html',
  styleUrls: ['./ibantools-page.component.scss']
})
export class IbantoolsPageComponent {
  constructor(private http: HttpClient,
    private cdRef: ChangeDetectorRef) {
  }
}
