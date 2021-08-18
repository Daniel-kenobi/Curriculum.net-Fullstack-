import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { usuarioModel } from '../../models/usuario.model';
import { authService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {

  loginModel: FormGroup;

  constructor(private frmBuilder: FormBuilder, private service: authService) { }

  ngOnInit() {
    this.loginModel = this.frmBuilder.group({
      Email: ['', [Validators.required, Validators.email]],
      Senha: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  fnc_cria_modelo(): usuarioModel {
    const usuario = new usuarioModel();
    usuario.Email = this.loginModel.controls['Email'].value;
    usuario.Senha = this.loginModel.controls['Senha'].value;

    return usuario;
  }

  fnc_login() {
    this.service.fnc_logar(this.fnc_cria_modelo()).subscribe(x => {
      this.service.fnc_altera_usuario_logado(x);
    }, (err) => console.log(err))
  }
}
