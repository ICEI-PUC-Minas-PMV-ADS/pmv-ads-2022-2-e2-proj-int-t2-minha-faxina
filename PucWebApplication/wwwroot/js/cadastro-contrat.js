function mascaraCep(cep) {
    const textoAtual = cep.value;
    let textoAjustado;
    textoAjustado = textoAtual.replace(/^(\d{5})(\d{3})/, "$1-$2");
    cep.value = textoAjustado;
}

function mascaraCpf(cpf) {
    const textoAtual = cpf.value;
    let textoAjustado;
    textoAjustado = textoAtual.replace(/^(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
    cpf.value = textoAjustado;
}