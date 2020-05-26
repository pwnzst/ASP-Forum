document.addEventListener("DOMContentLoaded", () => {
    const authSend =
        document
            .querySelector(".auth-block")
            .querySelector("input[type=button]");

    if (authSend)
        authSend.addEventListener('click', SendAuthorization);

    const hideButtons = document.querySelectorAll(".post-control-hide");
    for (let hb of hideButtons) {
        hb.onclick = hideButtonClick;
    }

    const pinButtons = document.querySelectorAll(".post-control-pin");
    for (let pb of pinButtons) {
        pb.onclick = pinButtonClick;
    }

    const replyButtons = document.querySelectorAll(".post-control-reply");
    for (let rb of replyButtons) {
        rb.onclick = replyButtonClick;
    }

    const editButtons = document.querySelectorAll(".personal-control-edit");
    for (let eb of editButtons) {
        eb.onclick = editButtonClick;
    }

    const smileImgs = document.querySelectorAll(".smiley-img");
    for (let si of smileImgs) {
        si.onclick = smileyClick;
    }

    const cabinetSpans = document.querySelectorAll("#cabinetName,#cabinetEmail,#cabinetStatus");
    for (let cabinetSpan of cabinetSpans) {
        cabinetSpan.onclick = cabinetNameClick;
    }

    const shownDataRadio = document.getElementsByName("ShownData");
    for (let sr of shownDataRadio) {
        sr.onchange = showDataClick
    }

    const avaFile = document.getElementsByName("AvatarFile");
    if (avaFile.length > 0) {
        avaFile[0].onchange = showSaveButton;
    }

    const saveUserEdit = document.getElementById("saveUserEdit");
    if (saveUserEdit)
        saveUserEdit.addEventListener('click', saveUserEditClick)

    const passwordEdit = document.getElementById("PassHash");
    if (passwordEdit)
        passwordEdit.addEventListener('blur', showSaveButton);

    const addThemeSectionButton = document.querySelector(".add-theme-button");
    if (addThemeSectionButton) {
        addThemeSectionButton.addEventListener('click', addThemeSectionClick)
    }

    const sectionInput = document.querySelector('.add-section-input');
    if (sectionInput) {
        sectionInput.addEventListener('change', sectionInputChange)
    }
});

function sectionInputChange(e) {
    // console.log(e.target.value)
    var datalist = document.getElementById("sections");
    var opt = datalist.querySelectorAll("option");
    var valInList = false;
    for (let o of opt) {
        if (e.target.value == o.value) valInList = true;
    }
    let i = document.getElementById("newSectionDescription");        
    if (!valInList) {
        // Введено имя для нового раздела
        i.innerHTML = "Описание: <input class='add-section-description-input'/>";
    } else {
        i.innerHTML = "Выберите раздел или впишите для создания нового";
    }
}

function addThemeSectionClick() {
    const sectionInput = document.querySelector('.add-section-input');
    if (!sectionInput) {
        console.error('.add-section-input locate error');
        return;
    }
    const themeInput = document.querySelector('.add-theme-input');
    if (!themeInput) {
        console.error('.add-theme-input locate error');
        return;
    }
    const descriptionInput = document.querySelector('.add-theme-description-input');
    if (!descriptionInput) {
        console.error('.add-theme-description-input locate error');
        return;
    }
    const section = sectionInput.value.trim();    
    if (section.length == 0) {
        alert('Укажите раздел');
        return;
    }
    const description = descriptionInput.value.trim();
    if (description.length == 0) {
        alert('Укажите описание темы');
        return;
    }
    const theme = themeInput.value.trim();
    if (theme.length == 0) {
        alert('Укажите заголовок темы');
        return;
    }
    let secDescription='';
    const sectionDescriptionInput = document.querySelector('.add-section-description-input');
    if (sectionDescriptionInput) {
        secDescription = sectionDescriptionInput.value;
    }
    // alert(section+'\r\n'+theme)
    const x = new XMLHttpRequest();
    x.onreadystatechange = () => {
        if (x.readyState === 4) {
            // console.log(x.responseText);
            var thId = parseInt(x.responseText);
            if (thId > 0) {
                window.location = "/Home/About/" + thId;
            } else {
                if (thId == -5) {
                    alert("Тема уже существует в разделе");
                } else {
                    console.error("response error No " + thId);
                }
            }
        }
    }
    x.open('POST', "Home/AddTheme", true);

    //x.setRequestHeader('Content-Type','application/json');
    //x.send('{"section":"' + section + '", "theme":"' + theme + '"}');

    x.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    x.send(
        "section=" + section
        + "&theme=" + theme
        + "&description=" + description
        + "&secDescription=" + secDescription
    );
}

function saveUserEditClick() {
    const form = document.createElement("FORM");
    form.setAttribute('action', '/Home/EditUser');
    form.setAttribute('method', 'POST');
    form.setAttribute('enctype', 'multipart/form-data');
    var name = document.createElement("INPUT");
    name.setAttribute('type', 'hidden');
    name.setAttribute('name', 'RealName');
    name.value = document.getElementById("cabinetName").innerText;
        form.appendChild(name);
    var pass = document.createElement("INPUT");
    pass.setAttribute('type', 'hidden');
    pass.setAttribute('name', 'PassHash');
    pass.value = document.getElementById("PassHash").value;
        form.appendChild(pass);
    var email = document.createElement("INPUT");
    email.setAttribute('type', 'hidden');
    email.setAttribute('name', 'Email');
    email.value = document.getElementById("cabinetEmail").innerText;
        form.appendChild(email);
    var stat = document.createElement("INPUT");
    stat.setAttribute('type', 'hidden');
    stat.setAttribute('name', 'Status');
    stat.value = document.getElementById("cabinetStatus").innerText;
        form.appendChild(stat);
    var sd = document.createElement("INPUT");
    sd.setAttribute('type', 'hidden');
    sd.setAttribute('name', 'ShownData');
    sd.value = document.querySelector("input[name=ShownData]:checked").value;
        form.appendChild(sd);

    form.appendChild(document.querySelector('input[name="AvatarFile"]'))

    document.body.appendChild(form);
    form.submit();
}

showDataClick = function (e) {
    showSaveButton()
}
// 1 - Status, 2 - radio, 3 radioStyle
// Д. З. 1. Добавить кнопку "Сохранить изменения" после активации любого изменения
// 2. 
cabinetNameClick = (e) => {
    const i = document.createElement("INPUT");
    i.value = e.target.innerText;
    i.style.width = e.target.offsetWidth + 10 + 'px';
    i.addEventListener('blur', nameBlur);
    e.target.savedDisplay = e.target.style.display;
    e.target.style.display = 'none';
    e.target.parentNode.insertBefore(i, e.target);
    i.focus();
}
nameBlur = (e) => {
    const i = e.target;
    const s = i.nextSibling;

    if (s.innerText != i.value) {
        showSaveButton();
    }
    s.innerText = i.value;
    s.style.display = s.savedDisplay;
    i.parentNode.removeChild(i);    
}

function showSaveButton() {
    const sb = document.getElementById("saveUserEdit");
    if (sb) 
        sb.style.display = "block";
    else
        console.error("Control location error: saveUserEdit")
}

editButtonClick = (e) => {
    window.location.hash = "";
    const block = e.target.closest(".post-block");
    const txt = block.querySelector(".post-message span");
    const msg = document.querySelector('textarea[name="Message"]');
    msg.value = txt.innerText;
    const postForm = document.getElementById("post-form");
    const idInput = postForm.querySelector("input[name=Id]");
    idInput.value = block.querySelector("cite").innerText;

    if (postForm.querySelector("input[value=Отмена]") == null) {
        const cancelBtn = document.createElement("INPUT");
        cancelBtn.type = "button";
        cancelBtn.value = "Отмена";
        cancelBtn.onclick = cancelClick;
        postForm.appendChild(cancelBtn);
    }
    window.location.hash = "post-form";  
    msg.focus();
}

function cancelClick() {
    const msg = document.querySelector('textarea[name="Message"]');
    const postForm = document.getElementById("post-form");
    const idInput = postForm.querySelector("input[name=Id]");
    idInput.value = '0';
    msg.value = '';
    postForm.removeChild(this);
    window.location.hash = "";
}

replyButtonClick = (e) => {
    window.location.hash = "";
    const block = e.target.closest(".post-block");
    const txt = block.querySelector(".post-message span");
    const nik = block.querySelector(".post-user-info b");
    const msg = document.querySelector('textarea[name="Message"]');
    const idCite = document.querySelector("input[name=Id_Cite]");
    idCite.value = block.querySelector("cite").innerText;
    window.cite_msg.innerHTML =
        "<div class='del-cite' onclick='this.parentNode.innerHTML=``;document.querySelector(`input[name=Id_Cite]`).value=0'></div>" +
        "Ответ на сообщение - <b>" +
        nik.firstChild.wholeText.trim() + '</b>: <i>' + txt.innerText.trim() + '</i>';
    window.location.hash = "post-form";  // Работает на изменение - нужно в первой строке сбросить
    msg.focus();
    // msg.click();
    // window.location = window.location;  // Перезагрузка (обновление) страницы
}

pinButtonClick = (e) => {
    const block = e.target.closest(".post-block");  // поиск родителя с заданным селектором
    if (block.style.position == "fixed")
    {
        block.style.zIndex = "0";
        block.style.position = "relative";
        e.target.innerText = "Pin";
        e.target.style['background-position-x'] = "5px";
    } else {
        block.style.zIndex = "1";
        block.style.position = "fixed";
        e.target.innerText = "UnPin";
        e.target.style['background-position-x'] = "-23px";
    }
    
}

hideButtonClick = (e) => {    
    const block = e.target.closest(".post-block");  // поиск родителя с заданным селектором
    block.style.display = "none";
}

function SendAuthorization(e) {
    const loginInput = document.querySelector("input[name=UserLogin]");
    const passwordInput = document.querySelector("input[name=UserPassword]");
    if (loginInput.value.length < 2) {
        alert("Login is too short");
        return;
    }
    if (passwordInput.value.length < 1) {
        alert("Enter password")
        return;
    }
    e.target.closest("FORM").submit();
}

SendClick = (e) => {
    const loginInput = document.querySelector("input[name=Login]");
    if (!loginInput) {
        console.log("Error input(Login) location");
        return;
    }
    if (loginInput.value.length < 2) {
        alert("Login too short");
        return;
    }
    const sd = document.querySelector("input[name=ShownData]:checked");
    if (!sd) {
        alert("Select ShownData");
        return;
    }
    const avaInput = document.querySelector("input[name=AvatarFile]");
    if (!avaInput) {
        console.log("Error input(AvatarFile) location");
        return;
    }
    console.log(avaInput.value);
    // return;
    e.target.closest("FORM").submit();
}

function smileyClick(e) {
    // alert(e.target.closest('.smiley-img').attributes["combo"].value)
    // Д.З. Подставить заменную комбинацию в текстареа, оставить фокус ввода
    var temp = document.querySelector('textarea[name="Message"]');
    temp.value += e.target.closest('.smiley-img').attributes["combo"].value;
    temp.focus();
}