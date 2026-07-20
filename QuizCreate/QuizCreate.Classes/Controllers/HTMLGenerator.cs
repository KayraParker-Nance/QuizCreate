using QuizCreate.Classes.Models;
using System.Text.Json;

namespace QuizCreate.Classes.Controllers
{
    public static class HTMLGenerator
    {
        public static string Generate(ParseResult parseResult, string title, Theme theme)
        {
            string sectionJson = JsonSerializer.Serialize(
                parseResult.Sections.Select( s => new
                {
                    name = s.Name,
                    questions = s.Questions.Select(q=> new
                    {
                        q = q.Text,
                        opts = q.Options,
                        ans = q.AnswerIndex,
                        exp = q.Explanation
                    })
                }),
                new JsonSerializerOptions { WriteIndented = false }
                );
            return FormatVariables(Template, theme, sectionJson, title);
        }

        private static string FormatVariables(string s, Theme theme, string sectionJson, string title)
        {
            return s
                .Replace("__TITLE__", title.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;"))
                .Replace("__SECTIONS__", sectionJson)
                .Replace("__BG__", theme.Background)
                .Replace("__PC__", theme.PrimaryColour)
                .Replace("__SC__", theme.SecondaryColour)
                .Replace("__TEXT__", theme.Text)
                .Replace("__MUTED__", theme.Muted)
                .Replace("__CORRECT__", theme.Correct)
                .Replace("__WRONG__", theme.Wrong)
                .Replace("__CARD__", theme.Card);
        }

        private const string Template = """
            <!DOCTYPE html>
            <html land="en">
            <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, intial-scale=1.0">
            <title>__TITLE__</title>
            <link href="https://fonts.googleapis.com/css2?family=Playfair+Display:wght@700&family=DM+Sans:wght@300;400;500&display=swap" rel="stylesheet">
            <style>
            :root{--background-colour:__BG__;--primary-colour:__PC__;--secondary-colour:__SC__;--text:__TEXT__;--muted:__MUTED__;--correct:__CORRECT__;--wrong:__WRONG__;--card:__CARD__}
            *{box-sizing:border-box;margin:0;padding:0}
            body{font-family:'DM Sans',sans-serif;background:var(--background-colour);color:var(--text);min-height:100vh}
            header{background:var(--secondary-colour);color:var(--background-colour);text-align:center;padding:2.5rem 1rem 2rem}
            header h1{font-family:'Playfair Display',serif;font-size:clamp(1.8rem,4vw,2.8rem)}
            header p{margin-top:.5rem;color:var(--primary-colour);font-weight:300;font-size:.95rem}
            .section-tabs{display:flex;flex-wrap:wrap;gap:.5rem;padding:1.2rem 1rem;background:var(--text);justify-content:center;position:sticky;top:0;z-index:100}
            .tab-btn{background:transparent;border:1px solid var(--text);color:var(--primary-colour);padding:.4rem .9rem;border-radius:2rem;cursor:pointer;font-family:'DM Sans',sans-serif;font-size:.8rem;font-weight:500;transition:all .2s}
            .tab-btn:hover{border-color:var(--primary-colour);color:var(--background-colour)}
            .tab-btn.active{background:var(--primary-colour);color:var(--secondary-colour);border-color:var(--primary-colour);font-weight:700}
            .shuffle-btn {
                background: transparent;
                border: 1px solid var(--text);
                color: var(--primary-colour);
                padding: .4rem .9rem;
                border-radius: 2rem;
                cursor: pointer;
                font-family: 'DM Sans', sans-serif;
                font-size: .8rem;
                font-weight: 500;
                transition: all .2s;
                margin-left: .5rem;
            } 
            .shuffle-btn:hover  { border-color: var(--primary-colour); color: var(--background-colour); }
            .shuffle-btn.active { background: var(--primary-colour); color: var(--text); border-color: var(--primary-colour); font-weight: 700; }
            .section-header{text-align:center;padding:2rem 1rem 1rem}
            .section-header h2{font-family:'Playfair Display',serif;font-size:1.7rem;color:var(--secondary-colour)}
            .section-header p{color:var(--muted);margin-top:.3rem;font-size:.9rem}
            .progress-bar-wrap{background:var(--muted);height:6px;border-radius:3px;margin:.8rem auto;max-width:400px}
            .progress-bar{height:6px;background:var(--primary-colour);border-radius:3px;transition:width .4s ease}
            .stats{display:flex;justify-content:center;gap:1.5rem;font-size:.85rem;color:var(--muted);margin-bottom:1.5rem}
            .stats span b{color:var(--text)}
            .quiz-container{max-width:700px;margin:0 auto;padding:0 1rem 3rem}
            .question-card{background:var(--card);border-radius:16px;padding:1.8rem;margin-bottom:1.2rem;box-shadow:0 2px 12px rgba(0,0,0,.07);border:1px solid var(--muted)}
            .q-number{font-size:.72rem;font-weight:700;letter-spacing:.12em;text-transform:uppercase;color:var(--primary-colour);margin-bottom:.6rem}
            .q-text{font-size:1.05rem;font-weight:500;line-height:1.6;margin-bottom:1.1rem}
            .options{display:flex;flex-direction:column;gap:.55rem}
            .opt-btn{background:white;border:1.5px solid var(--muted);border-radius:10px;padding:.75rem 1rem;cursor:pointer;text-align:left;font-family:'DM Sans',sans-serif;font-size:.92rem;color:var(--text);transition:all .18s;line-height:1.45}
            .opt-btn:hover:not(:disabled){border-color:var(--primary-colour);background:var(--muted)}
            .opt-btn.correct{background:color-mix(in srgb, var(--correct) 15%, var(--card));border-color:var(--correct);color:var(--correct);font-weight:600}
            .opt-btn.wrong{background:color-mix(in srgb, var(--wrong) 15%, var(--card));border-color:var(--wrong);color:var(--wrong)}
            .opt-btn.reveal{background:color-mix(in srgb, var(--correct) 15%, var(--card));border-color:var(--correct);color:var(--correct);opacity:.7}
            .opt-btn:disabled{cursor:default}
            .explanation{margin-top:.9rem;padding:.75rem 1rem;background:var(--background-colour);border-left:3px solid var(--primary-colour);border-radius:0 8px 8px 0;font-size:.88rem;color:var(--muted);display:none;line-height:1.6}
            .explanation.show{display:block}
            .results-panel{background:var(--secondary-colour);color:var(--background-colour);border-radius:16px;padding:2rem;text-align:center;margin-bottom:1.5rem}
            .results-panel h3{font-family:'Playfair Display',serif;font-size:1.5rem;margin-bottom:.5rem}
            .score-big{font-size:3.5rem;font-weight:700;color:var(--primary-colour);line-height:1;margin:.5rem 0}
            .reset-btn{margin-top:1.2rem;background:var(--primary-colour);border:none;color:var(--secondary-colour);padding:.7rem 1.8rem;border-radius:2rem;font-family:'DM Sans',sans-serif;font-weight:700;font-size:.9rem;cursor:pointer}
            </style>
            </head>
            <body>
            <header>
              <h1>__TITLE__</h1>
              <p>Multiple choice — select an answer to reveal feedback</p>
            </header>
            <div class="section-tabs" id="tabs">
            <button class="shuffle-btn" id="shuffle-btn" onclick="toggleShuffle()">🔀 Shuffle</button></div>
            <div class="section-header">
              <h2 id="section-title"></h2>
              <p id="section-desc"></p>
              <div class="progress-bar-wrap"><div class="progress-bar" id="prog-bar"></div></div>
              <div class="stats">
                <span>Questions: <b id="stat-q">0</b></span>
                <span>Correct: <b id="stat-c" style="color:var(--correct)">0</b></span>
                <span>Wrong: <b id="stat-w" style="color:var(--wrong)">0</b></span>
              </div>
            </div>
            <div class="quiz-container" id="quiz-area"></div>
            <script>
            const rawSections=__SECTIONS__;
            function buildAll(shuffle) {
                let all = rawSections.flatMap(s => s.questions);
                if (shuffle) {
                    for (let i = all.length - 1; i > 0; i--) {
                        const j = Math.floor(Math.random() * (i + 1));
                        [all[i], all[j]] = [all[j], all[i]];
                    }
                }
                return { name: 'All', questions: all };
            }

            let shuffled = false;
            const sections = [buildAll(false), ...rawSections];
            let current=0,answered={},scores={};
            sections.forEach(s=>{scores[s.name]={correct:0,wrong:0}});
            function buildTabs(){const t=document.getElementById('tabs');sections.forEach((s,i)=>{const b=document.createElement('button');b.className='tab-btn'+(i===0?' active':'');b.textContent=s.name;b.onclick=()=>switchSection(i);t.appendChild(b);});}
            function switchSection(i){current=i;document.querySelectorAll('.tab-btn').forEach((b,j)=>b.classList.toggle('active',i===j));render();}
            function render(){const s=sections[current];document.getElementById('section-title').textContent=s.name;document.getElementById('section-desc').textContent=s.questions.length+' questions';updateStats();const area=document.getElementById('quiz-area');area.innerHTML='';s.questions.forEach((q,qi)=>{const key=s.name+':'+qi;const card=document.createElement('div');card.className='question-card';const opts=q.opts.map((o,oi)=>{const st=answered[key];let cls='';if(st!==undefined){if(oi===q.ans)cls=st===oi?'correct':'reveal';else if(oi===st)cls='wrong';}return'<button class="opt-btn '+cls+'" '+(st!==undefined?'disabled':'')+' onclick="pick(\''+key+'\','+oi+','+q.ans+',\''+s.name.replace(/'/g,"\\'")+'\''+',this.closest(\'.question-card\'))">'+o+'</button>';}).join('');const expShow=answered[key]!==undefined?'show':'';card.innerHTML='<div class="q-number">Q'+(qi+1)+' of '+s.questions.length+'</div><div class="q-text">'+q.q+'</div><div class="options">'+opts+'</div>'+(q.exp?'<div class="explanation '+expShow+'">💡 '+q.exp+'</div>':'');area.appendChild(card);});updateProg();checkDone();}
            function pick(key,chosen,correct,sec,card){if(answered[key]!==undefined)return;answered[key]=chosen;card.querySelectorAll('.opt-btn').forEach((b,i)=>{b.disabled=true;if(i===correct)b.className=chosen===i?'opt-btn correct':'opt-btn reveal';else if(i===chosen)b.className='opt-btn wrong';});const exp=card.querySelector('.explanation');if(exp)exp.classList.add('show');if(chosen===correct)scores[sec].correct++;else scores[sec].wrong++;updateStats();updateProg();checkDone();}
            function updateStats(){const s=sections[current];const sc=scores[s.name];const n=Object.keys(answered).filter(k=>k.startsWith(s.name+':')).length;document.getElementById('stat-q').textContent=n+'/'+s.questions.length;document.getElementById('stat-c').textContent=sc.correct;document.getElementById('stat-w').textContent=sc.wrong;}
            function updateProg(){const s=sections[current];const n=Object.keys(answered).filter(k=>k.startsWith(s.name+':')).length;document.getElementById('prog-bar').style.width=(n/s.questions.length*100)+'%';}
            function checkDone(){const s=sections[current];const n=Object.keys(answered).filter(k=>k.startsWith(s.name+':')).length;if(n!==s.questions.length)return;const area=document.getElementById('quiz-area');if(area.querySelector('.results-panel'))return;const sc=scores[s.name];const pct=Math.round(sc.correct/s.questions.length*100);const grade=pct>=90?'🏆 Excellent!':pct>=75?'✅ Good work!':pct>=60?'📚 Keep studying!':'🔄 Review needed';const p=document.createElement('div');p.className='results-panel';p.innerHTML='<h3>Section Complete!</h3><div class="score-big">'+pct+'%</div><div style="color:var(--background-colour)">'+sc.correct+'/'+s.questions.length+' correct — '+grade+'</div><button class="reset-btn" onclick="resetSection()">🔄 Retry</button>';area.insertBefore(p,area.firstChild);p.scrollIntoView({behavior:'smooth',block:'start'});}
            function toggleShuffle() {
                shuffled = !shuffled;
                // Rebuild the All section with new shuffle state
                sections[0] = buildAll(shuffled);
                // Clear answers for All section only
                Object.keys(answered).forEach(k => { if (k.startsWith('All:')) delete answered[k]; });
                scores['All'] = { correct: 0, wrong: 0 };
                document.getElementById('shuffle-btn').classList.toggle('active', shuffled);
                // If currently on All, re-render
                if (current === 0) render();
            }
            function resetSection(){const s=sections[current];s.questions.forEach((_,i)=>{delete answered[s.name+':'+i];});scores[s.name]={correct:0,wrong:0};render();window.scrollTo({top:0,behavior:'smooth'});}
            buildTabs();render();
            </script>
            </body>
            </html>
            """;
    }
}
