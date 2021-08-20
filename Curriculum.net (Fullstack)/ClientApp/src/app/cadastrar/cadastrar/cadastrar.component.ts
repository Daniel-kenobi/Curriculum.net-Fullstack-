import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { cadastroModel } from '../../models/cadastro.model';
import { usuarioModel } from '../../models/usuario.model';
import { authService } from '../../services/auth.service';

@Component({
  selector: 'app-cadastrar',
  templateUrl: './cadastrar.component.html',
})
export class CadastrarComponent implements OnInit {
  cadastrarForm: FormGroup;

  constructor(private frmBuilder: FormBuilder, private service: authService, private router: Router) { }

  ngOnInit() {
    this.cadastrarForm = this.frmBuilder.group({
      Nome: ['', [Validators.required, Validators.minLength(3)]],
      Email: ['', [Validators.required, Validators.email]],
      Senha: ['', [Validators.required, Validators.minLength(6)]],
      ConfirmacaoSenha: ['', [Validators.required, Validators.minLength(6)]]
    })
  }

  fnc_monta_modelo(): cadastroModel {
    const cadastro = new cadastroModel();
    cadastro.Nome = this.cadastrarForm.controls['Nome'].value;
    cadastro.Email = this.cadastrarForm.controls['Email'].value;
    cadastro.Senha = this.cadastrarForm.controls['Senha'].value;
    cadastro.ConfirmacaoSenha = this.cadastrarForm.controls['ConfirmacaoSenha'].value;

    return cadastro;
  }

  fnc_cria_usuario(usr: cadastroModel): usuarioModel {
    const rst = new usuarioModel();
    rst.Nome = usr.Nome;
    rst.Email = usr.Email;

    return rst;
  }

  fnc_cadastra() {
    this.service.fnc_cadastrar(this.fnc_monta_modelo()).subscribe(x => {
      this.service.fnc_altera_usuario_logado(this.fnc_cria_usuario(x));
      this.router.navigateByUrl('home');
    }),
      (err) => console.log(err)
  }

}
