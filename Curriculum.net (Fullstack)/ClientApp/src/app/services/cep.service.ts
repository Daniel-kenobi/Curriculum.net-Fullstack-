import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { dto_endereco } from "../models/curriculo.model";

@Injectable({
  providedIn: 'root'
})
export class cepService {
  url: string = "https://viacep.com.br/ws/";

  constructor(private http: HttpClient) {

  }

  fnc_busca_cep(cep: string): Observable<dto_endereco> {
    return this.http.get<dto_endereco>(this.url + `${cep}/json/`);
  }
}
