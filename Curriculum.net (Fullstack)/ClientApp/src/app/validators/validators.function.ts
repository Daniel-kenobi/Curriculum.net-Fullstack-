import { AbstractControl, ValidatorFn } from "@angular/forms";

export function senhaValidator(control: AbstractControl): ValidatorFn  {
  try {

    let Senha = control.get('Senha').value;
    let ConfirmacaoSenha = control.get('ConfirmacaoSenha').value
    return Senha === ConfirmacaoSenha ? null : { naoCoincide: true }

  } catch (e) {

  }

}
