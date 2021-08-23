import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { cadastroModel } from "../models/cadastro.model";
import { usuarioModel } from "../models/usuario.model";


@Injectable({
  providedIn: 'root'
})
export class authService implements OnInit {

  private baseUrl: string;
  public usrLogado: usuarioModel;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router: Router) {
    this.baseUrl = baseUrl + "v1/api/auth/";
  }

  ngOnInit() {

  }

  fnc_altera_usuario_logado(usuario: usuarioModel) {
    this.usrLogado = usuario;
  }

  fnc_retorna_usuario_logado(): usuarioModel {
    return this.usrLogado;
  }

  fnc_logar(modelo: usuarioModel): Observable<usuarioModel> {
    return this.http.post<usuarioModel>(`${this.baseUrl}login`, modelo);
  }

  fnc_cadastrar(modelo: cadastroModel): Observable<cadastroModel> {
    return this.http.post<cadastroModel>(`${this.baseUrl}cadastrar`, modelo);
  }

  fnc_logout() {
    this.router.navigateByUrl('home').then(x => window.location.reload())
  }
}
