import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Inject, Injectable, OnInit } from "@angular/core";
import { Observable } from "rxjs";
import { mensagemModel } from "../models/mensagem.model";
import { authService } from "./auth.service";



@Injectable({
  providedIn: 'root'
})
export class mensagemService implements OnInit {
  baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private authService: authService) {
    this.baseUrl = baseUrl + "v1/api/mensagem/";
  }

  ngOnInit() {

  }

  fnc_cria_header(): HttpHeaders {
    return new HttpHeaders().set("Authorization", "Bearer " + this.authService.fnc_retorna_token());
  }

  fnc_envia_mensagem(mensagem: mensagemModel): Observable<mensagemModel> {
    return this.http.post<mensagemModel>(`${this.baseUrl}enviar`, mensagem, { headers: this.fnc_cria_header() })
  }
}
