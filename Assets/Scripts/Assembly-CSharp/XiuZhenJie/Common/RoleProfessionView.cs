using UnityEngine;
using UnityEngine.UI;
using static XiuZhenJie.RoleFolio;

namespace XiuZhenJie
{
    internal sealed class RoleProfessionView : MonoBehaviour
    {
        private Image professionIcon;
        private Text helpCue;
        private Text experience;
        private Image track;
        private Image fill;

        internal static void Bind(CeHuaItem item, string name, string rank, long currentExp, long maxExp)
        {
            var row = item.GetComponent<RoleProfessionView>();
            if (row == null)
            {
                row = item.gameObject.AddComponent<RoleProfessionView>();
                row.Build(item);
            }
            item.TitleUIText.text = name;
            item.ValueText.text = rank;
            var art = InkInterfaceArt.Get("Professions/" + item.Profession);
            row.professionIcon.sprite = art;
            row.professionIcon.enabled = art != null;
            row.helpCue.gameObject.SetActive(item.HelpBtn.gameObject.activeSelf);
            bool hasLimit = maxExp > 0;
            row.experience.text = hasLimit ? $"{currentExp:N0} / {maxExp:N0}" : currentExp.ToString("N0");
            row.track.gameObject.SetActive(hasLimit);
            if (hasLimit)
                Span(row.fill.rectTransform, 0, Mathf.Clamp01((float)((double)currentExp / maxExp)));
        }

        private void Build(CeHuaItem item)
        {
            var rect = (RectTransform)transform;
            Height(rect, 72);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 72);
            var paper = GetComponent<Image>() ?? gameObject.AddComponent<Image>();
            Flat(paper, Card);
            var font = item.TitleUIText.font;
            professionIcon = RoleFolio.Image(rect, "职业图标", Color.white);
            professionIcon.preserveAspect = true;
            Fixed(professionIcon.rectTransform, 0, 4, 8, 42, 44);

            item.TitleUIText.rectTransform.SetParent(rect, false);
            item.ValueText.rectTransform.SetParent(rect, false);
            var oldGround = rect.Find("Value_bg");
            if (oldGround != null) oldGround.gameObject.SetActive(false);
            var top = Box(rect, "职业与品阶");
            Band(top, 53, 17, 5, 25);
            item.TitleUIText.rectTransform.SetParent(top, false);
            item.ValueText.rectTransform.SetParent(top, false);
            Span(item.TitleUIText.rectTransform, 0, .47f);
            Span(item.ValueText.rectTransform, .47f, 1);
            Style(item.TitleUIText, 16, Ink, TextAnchor.MiddleLeft, true, 12);
            Style(item.ValueText, 14, Jade, TextAnchor.MiddleRight, false, 11);
            helpCue = Label(rect, font, "›", 15, Secondary, TextAnchor.MiddleCenter);
            Fixed(helpCue.rectTransform, 1, -2, 5, 13, 25);

            var experienceLabel = Label(rect, font, "经验", 12, Secondary);
            Fixed(experienceLabel.rectTransform, 0, 53, 32, 32, 21);
            experience = Label(rect, font, "0", 13, Ink, TextAnchor.MiddleRight);
            Style(experience, 13, Ink, TextAnchor.MiddleRight, false, 10);
            Band(experience.rectTransform, 89, 13, 32, 21);
            track = RoleFolio.Image(rect, "经验进度", Rule);
            Band(track.rectTransform, 12, 13, 60, 3);
            fill = RoleFolio.Image(track.transform, "已获经验", Jade);
            Fill(fill.rectTransform);

            item.HelpBtn.transform.SetParent(rect, false);
            Fill((RectTransform)item.HelpBtn.transform);
            Touch(item.HelpBtn);
        }

    }
}
