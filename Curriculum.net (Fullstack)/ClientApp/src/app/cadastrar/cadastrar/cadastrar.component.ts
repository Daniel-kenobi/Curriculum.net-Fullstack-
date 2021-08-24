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
      Telefone: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(11)]],
      Cep: [''],
      Instagram: [''],
      Linkedin: [''],
      GitHub: [''],
      Email: ['', [Validators.required, Validators.email]],
      Senha: ['', [Validators.required, Validators.minLength(6)]],
      ConfirmacaoSenha: ['', [Validators.required, Validators.minLength(6)]]
    })
  }

  fnc_monta_modelo(): cadastroModel {
    const cadastro = new cadastroModel();
    cadastro.nome = this.cadastrarForm.controls['Nome'].value;
    cadastro.email = this.cadastrarForm.controls['Email'].value;
    cadastro.senha = this.cadastrarForm.controls['Senha'].value;
    cadastro.confirmacaoSenha = this.cadastrarForm.controls['ConfirmacaoSenha'].value;
    cadastro.telefone = this.cadastrarForm.controls['Telefone'].value;
    cadastro.instagram = this.cadastrarForm.controls['Instagram'].value;
    cadastro.linkedin = this.cadastrarForm.controls['Linkedin'].value;
    cadastro.github = this.cadastrarForm.controls['GitHub'].value;

    return cadastro;
  }

  fnc_cria_usuario(usr: cadastroModel): usuarioModel {
    const rst = new usuarioModel();
    rst.nome = usr.nome;
    rst.email = usr.email;
    rst.github = usr.github;
    rst.telefone = usr.telefone;
    rst.instagram = usr.instagram;
    rst.linkedin = usr.linkedin;

    return rst;
  }

  fnc_cadastra() {
    this.service.fnc_cadastrar(this.fnc_monta_modelo()).subscribe(x => {
      this.service.fnc_altera_usuario_logado(this.fnc_cria_usuario(x));
      this.router.navigateByUrl('home');
    }), (err) => console.log(err)
  }

}
