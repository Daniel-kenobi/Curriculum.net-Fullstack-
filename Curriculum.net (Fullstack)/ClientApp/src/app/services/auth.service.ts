import { HttpClient } from "@angular/common/http";
import { Inject, Injectable, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs";
import { cadastroModel } from "../models/cadastro.model";
import { usuarioModel } from "../models/usuario.model";
import jwtDecode, * as jwt_decode from "jwt-decode";

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

  fnc_logado(): boolean {
    if (this.usrLogado)
      return true;
    else
      return false;
  }

  fnc_altera_usuario_logado(usuario: usuarioModel) {
    this.usrLogado = usuario;
  }

  fnc_retorna_usuario_logado(): usuarioModel {
    return this.usrLogado;
  }

  fnc_transforma_modelos(modelo: cadastroModel): usuarioModel {
    const rst = new usuarioModel();
    rst.nome = modelo.nome;
    rst.telefone = modelo.telefone;
    rst.instagram = modelo.instagram;
    rst.linkedin = modelo.linkedin;
    rst.github = modelo.github;
    rst.email = modelo.email;
    rst.senha = modelo.senha;
    rst.img_perfil = modelo.img_perfil;

    return rst
  }

  fnc_token(modelo: usuarioModel) {
    return this.http.post<usuarioModel>(`${this.baseUrl}token`, modelo);
  }

  fnc_logar(modelo: usuarioModel): Observable<usuarioModel> {
    return this.http.post<usuarioModel>(`${this.baseUrl}login`, modelo);
  }

  getDecodedAccessToken(token: string): any {
    try {
      return jwtDecode(token);
    }
    catch (Error) {
      return null;
    }
  }

  fnc_processa_token(modelo: usuarioModel): void {
    let tokenInfo = this.getDecodedAccessToken(modelo.token);
    localStorage.setItem('usuario', modelo.token);

    console.log(tokenInfo);
  }

  fnc_cadastrar(modelo: cadastroModel): Observable<cadastroModel> {
    return this.http.post<cadastroModel>(`${this.baseUrl}cadastrar`, modelo);
  }

  fnc_retorna_token(): string {
    return localStorage.getItem('usuario');
  }

  fnc_atualiza_credenciais(modelo: cadastroModel): Observable<cadastroModel> {
    return this.http.post<cadastroModel>(`${this.baseUrl}atualizar`, modelo);
  }

  fnc_logout() {
    this.router.navigateByUrl('home').then(x => window.location.reload())
  }

  fnc_redireciona(): void {
    if (!this.fnc_retorna_usuario_logado())
      this.router.navigateByUrl('home')
  }
}
