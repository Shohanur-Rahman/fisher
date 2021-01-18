
function SpecialCharacterCheck(stringData) {

    var isSpecialCharacterExist = false;

    if (stringData != null && stringData != "")
    {
        //Text message for PPE image title validation
        //There’s an issue with the PPE requirements icon title.The following characters are permitted: A - Z, a - z, 0 - 9, - , ., |, ( ), /, ‘, &, < >
        //Text message for other image title validation
        //For image title only these characters are allowed: A - Z, a - z, 0 - 9, -, ., |, ( ), /, ', &, < >

        //Regular Exp Not Allowing All Special Characters With White Space
        //var specialCharacterRegEx = /\`|\~|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\""|\;|\:|\s/g;

        //Regular Exp Not Allowing Specifiq Special Characters are: |#$%@^`~><_
        //var specialCharacterRegEx = /\`|\~|\@|\#|\$|\%|\^|\(|\)|\[|\{|\]|\}|\||\\|\'|\<|\>|\_/g;

        //Only alpha numeric character, space ,one forward slash and dash are allowed.
        //var specialCharacterRegEx = /\`|\~|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\>|\?|\/|\""|\;|\:|\_/g;

        //For image title only these characters are allowed: A-Z, a-z, 0-9, -, ( ), /, ', &, < >
        //var specialCharacterRegEx = /\`|\~|\!|\@|\#|\$|\%|\^|\.|\*|\+|\=|\[|\{|\]|\}|\||\\|\,|\?|\/\/|\""|\;|\:|\_|\"/g;

        //For image title only these characters are allowed: A-Z, a-z, 0-9, -, ., |, ( ), /, ', &, < >
        var specialCharacterRegEx = /\`|\~|\!|\@|\#|\$|\%|\^|\*|\+|\=|\[|\{|\]|\}|\|\||\\|\?|\/\/|\""|\;|\:|\_|\"/g;

        if (stringData.length > 0 && specialCharacterRegEx.test(stringData) == true) {
            isSpecialCharacterExist = true;
        }
    }

    return isSpecialCharacterExist;
}