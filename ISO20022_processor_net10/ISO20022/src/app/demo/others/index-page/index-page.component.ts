import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CardComponent } from 'src/app/theme/shared/components/card/card.component';
import { debounceTime, distinctUntilChanged, map, Observable, OperatorFunction } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { from } from "linq-to-typescript"

import Prism from 'prismjs';

interface selectionItem {
  item: string;
}

interface xmlDefinitionFromApi {
  xmlDef: string;
}

@Component({
  selector: 'app-index-page',
  imports: [CommonModule, CardComponent, FormsModule, NgbTypeaheadModule],
  templateUrl: './index-page.component.html',
  styleUrls: ['./index-page.component.scss']
})
export class IndexPageComponent {

  constructor(private http: HttpClient, private cdRef: ChangeDetectorRef) {

    this.http.get<string[]>('/api/Validator/GetSchemaUrns').subscribe(urns => {
   this.urns = urns;
   var urn = this.getQueryParameter('urn');
   if (urn && from(urns).any(x => x == urn)) {
     this.model = urn;
     this.show_sample_xml({ item: urn });
     this.cdRef.detectChanges();
   }
  })
}

  show_sample_xml($event: selectionItem): void {
    if ($event) {
      this.http.get<xmlDefinitionFromApi>('/api/Validator/GetXmlByUrn?urn=' + $event.item)
        .subscribe(x => {
          this.xmlDefinition = x.xmlDef;
          Prism.highlightElement(document.getElementById('exampleArea'), false, () => { });
          this.cdRef.detectChanges();
        });
    }
  };

  private getQueryParameter(key: string): string {
    const parameters = new URLSearchParams(window.location.search);
    return parameters.get(key);
  }

  urns: string[] = [];

  model: string | undefined;
  xmlDefinition: string | undefined;

	search_urn: OperatorFunction<string, readonly string[]> = (text$: Observable<string>) =>
		text$.pipe(
			debounceTime(200),
			distinctUntilChanged(),
			map((term) =>
				term.length < 2 ? [] : this.urns.filter((v) => v.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10),
      ),
    );
}
