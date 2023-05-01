// @ts-check
// Note: type annotations allow type checking and IDEs autocompletion

const lightCodeTheme = require("prism-react-renderer/themes/github");
const darkCodeTheme = require("prism-react-renderer/themes/dracula");

/** @type {import('@docusaurus/types').Config} */
const config = {
  title: "Aris Studio",
  tagline: "Make your own Blue Archive story",
  favicon: "img/favicon.ico",

  // Set the production url of your site here
  url: "https://as.t14.me",
  // Set the /<baseUrl>/ pathname under which your site is served
  // For GitHub pages deployment, it is often '/<projectName>/'
  baseUrl: "/",

  // GitHub pages deployment config.
  // If you aren't using GitHub pages, you don't need these.
  organizationName: "dzaaaaaa", // Usually your GitHub org/user name.
  projectName: "ArisStudio", // Usually your repo name.

  onBrokenLinks: "throw",
  onBrokenMarkdownLinks: "warn",

  // Even if you don't use internalization, you can use this field to set useful
  // metadata like html lang. For example, if your site is Chinese, you may want
  // to replace "en" with "zh-Hans".
  i18n: {
    defaultLocale: "zh-cn",
    locales: ["zh-cn", "en"],
    localeConfigs: {
      "zh-cn": {
        label: "简体中文",
        htmlLang: "zh-CN",
      },
      en: {
        label: "English",
        htmlLang: "en-US",
      },
    },
  },

  markdown: {
    mermaid: true,
  },

  presets: [
    [
      "classic",
      /** @type {import('@docusaurus/preset-classic').Options} */
      ({
        docs: {
          sidebarPath: require.resolve("./sidebars.js"),
          // Please change this to your repo.
          // Remove this to remove the "edit this page" links.
          editUrl:
            "https://github.com/Tualin14/ArisStudio/tree/documentation/0.3",
        },
        blog: false,
        theme: {
          customCss: require.resolve("./src/css/custom.css"),
        },
      }),
    ],
  ],

  themeConfig:
    /** @type {import('@docusaurus/preset-classic').ThemeConfig} */
    ({
      // Replace with your project's social card
      image: "img/IMG_Aris_Studio_Logo.jpg",
      navbar: {
        title: "Aris Studio",
        logo: {
          alt: "Aris Studio Logo",
          src: "img/ArisStudioIcon.svg",
        },
        items: [
          {
            type: "docSidebar",
            sidebarId: "tutorialSidebar",
            position: "left",
            label: "Docs",
          },
          {
            type: "localeDropdown",
            position: "right",
          },
          {
            href: "https://github.com/Tualin14/ArisStudio/",
            label: "GitHub",
            position: "right",
          },
        ],
      },
      footer: {
        style: "dark",
        links: [
          {
            title: "Docs",
            items: [
              {
                label: "Aris Studio",
                to: "/docs/intro",
              },
            ],
          },
          {
            title: "Community",
            items: [
              {
                label: "GitHub",
                href: "https://github.com/Tualin14/ArisStudio/",
              },
              {
                label: "Discussions",
                href: "https://github.com/Tualin14/ArisStudio/discussions",
              },
            ],
          },
          {
            title: "Links",
            items: [
              {
                label: "bilibili",
                href: "https://space.bilibili.com/403504801",
              },
            ],
          },
        ],
        copyright: `Copyright © ${new Date().getFullYear()} Aris Studio, Made with ❤️ by Tualin14 & Contributors`,
      },
      prism: {
        theme: lightCodeTheme,
        darkTheme: darkCodeTheme,
      },
    }),

  themes: [
    "@docusaurus/theme-mermaid",
    [
      "@easyops-cn/docusaurus-search-local",
      /** @type {import("@easyops-cn/docusaurus-search-local").PluginOptions} */
      ({
        indexBlog: false,
        hashed: true,
        language: ["zh", "en"],
        highlightSearchTermsOnTargetPage: true,
        explicitSearchResultPath: true,
      }),
    ],
  ],
};

module.exports = config;
