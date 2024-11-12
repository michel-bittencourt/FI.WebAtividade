function formataCPF(input) {
    let value = limparEntrada(input.value);
    value = aplicarMascara(value);
    input.value = value;
    validarCPFCompleto(input, value);
}

// Limpa a entrada
function limparEntrada(value) {
    return value.replace(/\D/g, "");
}

// Mascara
function aplicarMascara(value) {
    if (value.length > 11) {
        value = value.slice(0, 11);
    }
    if (value.length > 9) {
        value = value.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
    }
    else if (value.length > 6) {
        value = value.replace(/(\d{3})(\d{3})(\d{0,3})/, "$1.$2.$3");
    }
    else if (value.length > 3) {
        value = value.replace(/(\d{3})(\d{0,3})/, "$1.$2");
    }

    return value;
}

// Valida
function validarCPFCompleto(input, value) {
    if (value.length === 14) {
        input.setCustomValidity("");
    } else {
        input.setCustomValidity("Por favor, preencha o CPF completo.");
    }
}