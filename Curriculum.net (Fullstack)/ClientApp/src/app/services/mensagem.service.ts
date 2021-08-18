import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, OnInit } from "@angular/core";
import { Observable } from "rxjs";
import { mensagemModel } from "../models/mensagem.model";



@Injectable({
  providedIn: 'root'
})
export class mensagemService implements OnInit {
  baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  ngOnInit() {

  }

  fnc_envia_mensagem(mensagem: mensagemModel): Observable<mensagemModel> {
    return this.http.post<mensagemModel>(this.baseUrl + `api/mensagem/inc`, mensagemModel)
  }
}
