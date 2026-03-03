import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CardComponent } from 'src/app/theme/shared/components/card/card.component';
import { debounceTime, distinctUntilChanged, map, Observable, OperatorFunction } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { from } from "linq-to-typescript"
import { NgSelectModule } from '@ng-select/ng-select';

interface selectionItem {
  twoLetterISORegionName: string;
  englishName: string;
  nativeName: string;
}

interface ukResult {
  iban: string;
}

interface ibanCheck {
  error: string;
  isValid: boolean;
  attemptedValue: string;
}

@Component({
  selector: 'app-ibantools-page',
  imports: [CommonModule, CardComponent, FormsModule, NgbTypeaheadModule, NgSelectModule],
  templateUrl: './ibantools-page.component.html',
  styleUrls: ['./ibantools-page.component.scss']
})
export class IbantoolsPageComponent {
  constructor(private http: HttpClient,
    private cdRef: ChangeDetectorRef) {

    this.http.get<selectionItem[]>('/api/IbanCheck/IbanSupportedContries', {
      cache: 'no-cache',
    }).subscribe(cntry => {
      this.supportedCountries = cntry;
    })
  }

  public supportedCountries: selectionItem[] = [];
  public country: string | undefined;
  public generatedIban: string;
  public ibantobevalidated: string = "";
  public outputText: string = "";

  public bicCode: string = "";
  public sortCode: string = "";
  public accountNumber: string = "";

  public ukAcctIban: string = "";

  public generateIban(): void {
    if (this.country) {
      this.http.get<string>('/api/IbanCheck/IbanGenerator?countryCode=' + this.country, {
        cache: 'no-cache',
      }).subscribe(iban => {
        this.generatedIban = iban;
        this.cdRef.detectChanges();
      })
    } else {
      console.warn("No country selected");
    }
  }

  public validateIban(): void {
    this.outputText = "";

    if (this.ibantobevalidated) {
      this.http.get<ibanCheck>('/api/IbanCheck/Index?iban=' + this.ibantobevalidated, {
        cache: 'no-cache',
      })
        .subscribe({
          next: (iban) => {
            this.outputText = "Valid!";

            this.cdRef.detectChanges();
          },
          error: (iban) => {
            this.outputText = iban.error.error;

            this.cdRef.detectChanges();
          }
        })
    }
  }

  // Bank of England	GB91BKEN10000041610008
  // BKEN   100000    41610008
  public convertUKaccnt(): void {
    if (this.bicCode && this.sortCode && this.accountNumber) {
      this.http.get<ukResult>('/api/IbanCheck/UkAccntToIbanCalc?bic=' + this.bicCode + '&sortcode=' + this.sortCode + '&accountNr=' + this.accountNumber, {
        cache: 'no-cache',
      })
        .subscribe((res) => {
          this.ukAcctIban = res.iban;

          this.cdRef.detectChanges();
        });
    }
  }
}
