import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CurriculumModel } from "../models/curriculo.model";

@Injectable({
  providedIn: 'root'
})
export class curriculoService {
  baseUrl: string;

  constructor(@Inject('BASE_URL') baseUrl: string, private http: HttpClient) {
    this.baseUrl = baseUrl;
  }

  fnc_cria_curriculo(curriculo: CurriculumModel): Observable<any> {
    const httpOptions = {
      'responseType': 'arraybuffer' as 'json'
    };

    return this.http.post<any>(`${this.baseUrl}v1/api/inc`, curriculo as CurriculumModel, httpOptions);
  }
}
