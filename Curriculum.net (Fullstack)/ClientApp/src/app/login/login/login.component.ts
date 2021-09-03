import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { usuarioModel } from '../../models/usuario.model';
import { authService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {

  loginModel: FormGroup;
  erroLogin: object;

  constructor(private frmBuilder: FormBuilder, private authService: authService, private router: Router) { }

  ngOnInit() {
    this.loginModel = this.frmBuilder.group({
      Email: ['', [Validators.required, Validators.email]],
      Senha: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  fnc_cria_modelo(): usuarioModel {
    const usuario = new usuarioModel();
    usuario.email = this.loginModel.controls['Email'].value;
    usuario.senha = this.loginModel.controls['Senha'].value;

    return usuario;
  }

  fnc_login() {
    this.authService.fnc_logar(this.fnc_cria_modelo()).subscribe(x => {
      this.authService.usrLogado = x as usuarioModel;
      this.authService.fnc_processa_token(this.authService.usrLogado);
      this.router.navigateByUrl('home');

      this.erroLogin = null;
    }, (err) => { this.erroLogin = err })
  }
}
