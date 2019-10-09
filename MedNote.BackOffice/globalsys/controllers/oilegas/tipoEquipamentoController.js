'use strict';
Globalsys.controller('tipoEquipamentoController', ['$scope', 'implementoService', 'tipoEquipamentoService', '$uibModal', '$timeout', function ($scope, implementoService, tipoEquipamentoService, $uibModal, $timeout) {
    $scope.tiposEquipamento = [];
    $scope.tipoEquipamento = { Imagem: "", Extensao: "", Implementos: [] };
    var imagem_padrão = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAALQAAAC0CAIAAACyr5FlAAAABGdBTUEAAK/INwWK6QAAABl0RVh0U29mdHdhcmUAQWRvYmUgSW1hZ2VSZWFkeXHJZTwAACJoSURBVHja7J0JmBxVtcfrVlf13tM9S2YySSYTyL4QkrCFJSAIKLLIE8HHoqIsgsAT9Cmb76EQNlHAJ4siqIgCwtMPWcQHfAYCgRASCGEgCSEkmZBkMpml96W2+86tW91d3V3d05309PRk7vn6g05NdXV13d8953/OvXULqcucHDNmVsazS8CMwcGMwcGMwcGMwcGMwcGMwcGMwcGMwcGMwcGMwcGMGYODGYODGYODGYODGYODGYODGYODGYODGYODGYODGTMGBzMGBzMGBzMGBzMGBzMGBzMGBzMGx/5uiOeQYLFx/zWBNXr5dHCuqci/mGs8AblmcFoSB1/Dn93HyX05rGBt//nBbAmGsszmQwfchNov5GyenO2xLrz5BqwlEBI53s1hicCBEHHJsMXm5DS6hSds8Q6Ohy2ptMvh9S0OfQtKb7GTD8JxCHO87q5EDtk4TebkXhxexckDZB+s1IBCBkcZ0YTj0bS70MTLRvxcsDyo7HjUtv1WTo0yz1EHoQTcgGsaf9hq0oPrw9S+l9BHX4O4xgTpCPdVzuZF7RfVDxkkxLWcrHbezDzHiJqzE7Wejdq/xbkOrDtmNSmxbJLTFmHZSu2xmIImXILGX8DZW+tVCNmTthmc9K7Tjhkcw32xbRxWyRvPHDThYtR2Hif46/yUxXHHRT5Zy3Ha8PEx5jUHpIXETatcw2Fo1sP8IW+giZfXPxnEu006G/4bSfBJGTHPMTzJCNZQ4/HcxMtR86mjq9wp+g+G4ALiIxLnkVtziJjBUUXR70PNJ3PtFxE4Rmks9ByoRDbAu3CcbxgGPsYkHDYPaj0HTbiU8y0Y1b9DbFpM4aB8+D2aXagmH2NDc4DeNPpCAE26gj/kTTTzgdFOBphj/JfM/wzFeElBzHNUojfJMITKOdrR+AtJ0cLZsd/8OHvLcUQ2cVlvQeNLtfzHfgwH0serJDKUSooW53Niy/72CwUP72zXkjszWzDW44tbE6vBxyiBg+QRNjKAaQw45XSXIh9BEDiIt2g7B7Tn/toDxMCiVM9O8xbgI1QlPuoYDsHPuaYhz2zOu4BzHYjcM0FI4i034Z7HjGtQAozGz3ETLkPNpxhljP3XHK0npnqez9to8OHRRBsetXCYp8bYvGQsA1BwTiFlSvcMznkAyPH8T0z4Dt79pDElwuReibAwsDiB6/geajp5jCReYvORltsJHzE+4NGEfeCjNnDo4T/dfrpOdBIU7G2cezqgwLnnINcBnGOixTy8/B8t5ZABeMGh1LjxrwN+gjqvHVNZueCbhXgHzustaT6C+8ZHTeCwucgsKcgXXNM57zyCgmcmZ2/fmyq1efpTHhnt3xprZNAsXfAfLA+uKuE//HvLxzDD4ZiIpvwYeeaSBFIcl603VKV0QfRpPCNQ4Iu4MWkQWYrBAabR+OJVbXydwQFdmcy7rKZphsiAUJUhg0oNx4SxCYe96Yh46UsG8SVq2ws+htlzVD2H9MxDC/6JOPCSKqfG8NZbuMhast01jRurJjYeOnSXIvrDFvBUxkf14KCph81DBCayId6F1Wj1x74hfASOo4UOYlIv3ng5eaOExiwcvHMC72jTUruH4EOrmI/qwQFk2Mfx85/nvPPTKUoNLozD+PLoWsRxY9l5pHpeGNp/aKA/CB98eXxUc+ANNRyRIaNGlknhYl1csnvMwiH4DypzT1X3H5pWY89BpNH43JZLpN0Hzql55G3hXQVOSOGwbFEpsZgCnj6OGseRd5Fz8tiEA9kqmCVO+QB9yqNawpFpG7kPb/o+jr7PITtBhE7P1MMjSTS0eLZcwduR/yg09Q5DnWAFd9+Ne58CsZn+3YjjPToriMwF77zOYr4WqbDBL36NG3fm2IRD7l9Z0f6Ej+jQfFQVjvSwJ958Pe59Wr/1L5nT+3l7bn0TnIENxz/mPHPRpCvJB3v+iLfclNvwInEPenWV5CaeOSiHAN1zgApWInhwGQKG6ukGk9pYcsdfU7v/WemnVM2oj5Xgo1qaQ/8Gocno/dF1+g2fufVywZcTLGxuUjbV9C1K0Ng48FLOMW1eY0JGxmIf5uXKZDdNIpQkPuGAszFmie7HQmsu2rvPKioCPkqMYFZVkNrHGSIRHAbRDUqWDGhjyGyz0cRF2lWLGV0/fXsITu0ykeEhf83GF95wNjlfKZB9qH/CKg69OXawUMJdgyu+FH7v8pzOUzkfweJ8VAsOTNpJHEfPmpN2Z6WiQUbMNADrJjubWRF1OMCvGC7EigzwIub0xPjaXN0aWjkWsNBSuyNd1/a/eozUt7wKkBX3H9XTHNCcQoCqUU6NlOsz9N6PqOcAFAAseigzGTS+GP0DWWcrei6DwysRfBfFaH80chfktj/ENt6hpXqrqWeBD31+EELDBIe9jRO8uuQYNFAoJAN8BpBBZnPhLCvEK3gMlwN/gk/l+QzIVkCakMS4WDwTyBHUOJf4lIt2cf7F+yUZqZ4XYx/fKQ+uHpZ8R7Hgo2pwIPt4o2JBV7ophwxoUZLlYqOvq2H9TyifDE4tTkY6WyEqh4QYHFyO9js45MFVsY/vTu1+MVsUGB4+6PzkDB/Vy1YyzlzqoQUMCzLM0YR4C0QalSQyepFDDpK8I8MBJQOiiWn0NX/aHx2ehYNktEjojf0JCzW+Nbz2qsEVp5O5gMNJhtF0Oh9V9xyYE5vTBAYNAVHaZ2Q8BLzXwcIgYzMCM0NGlhWb1UJHNj1bSWX2wfENSO7PnszolRdKJP7pg/HNv9KkwaEnyFWVD1r/qGoqC5rDgKOPJpZD+Aw1vcXmN8bPpF35CtRMhqFS40WzFbIGl4dL7cDhd0ZD46uJ7j9Fuq6X+ixcXXL7k/2vLYmuv1mTw/pwt1LLU8v4D6H6cKQ+y6l0UTKyXiSXDPiHe5qRg6SHWMm0QlIsMWWtoGYoFq4pRbIVkXyXGjfq6M1frHM24pvvi3x4I6libflN45JXxMAio2H6V8Q2LJX6XjeCZm2xyIpfmfBRRc/RbjSXtDs/mhQng/PMRp3XG+077quo6SSiP2gJnK60B1hAjCBRxo5az0Kt51gJngwZVJO+PvRdLSOeeux+KZOdyhQF3ZuE11yUJoMfKTIyfFTJcyAe0Vl68HvkgaF0BuaEBtTxA9T0ec49IztbTGxE859FiS0cTpnqGTwZe4PDkgnrU4pkK1kyiCU2ccnP6vy2R3vLMVLfazRiis1HpXVGVFMimZrGiJ9kleAA8UgzDiVEqpyk6xcnA3zAvKdR4FjrQ7kOqARK0ZhMmiFDz5Jw+G1U33B4pv8nEgNKZIOz/Qyx8bB0yvoOlkOId5JJkJq8f8CBSF+ncJB6lFcvkso5CSdZq9XYgiZfW5SMyqnUx2wzZOirwAKgg69wrV+t67jCi+4DL8+TqPFP7tXHrkVOlerhHKsBh70VTbnBEKSCH027E29dyoF7pMUUOoeD+gyxFU34Nhr/jeppftNFpGTo6TEOvTX6hu8Rb289Uep/E5urAKMbDiTy857mGg7Lbmg5gyyhREZKdTiIYsAmiTpMHdFukEFFXGIzF9/IeebVGwBasie26efI5nJNucTm1udGabIcfJd3TbC5OjwzfiQH16Z2PZvO311YTY6guN5nODyzzGTkliVq5qLteq4by8p7rOLwO6ie4MBaMrHlkdgn9wAfhN7tT9hck5Hg0ZK7QHnYW44NHPkM+I6GBfcNhD9UY5uBjNrM0S4ds/fRc4zsbew4XQWJ5Uwfgcs68EodJa49Lw4sPz7SdS2QAXoTCT54Iw+ukvYsAzIQ75D6lkc//LHOeZP/0N8hMjFKw3QS02jWHCMaHY1sJZY/sUhL4vBbCLbX0oEVMTWxI7T6QqyLIUROT8Tme/XAQxDBnop/+oAYWOjsOFcMHOJf9BBZmMXRFl53tdz/FpGoI5G87LvnEEYaDmwx5Yz01l144OWRFxlyMPzeZflkpMcWdDJsOD0KHX7/anngbXjjaD/dPu4EoWFu4NBHbd5ppRcjqWfPMaJxMbk1tylS5inNeOvNKLCkOoNwxMknsJLAclBTwliJaMleTdqjpeDVh5WwJg3a3J2e6d+3eQ6g+yuxT6Q9rya2/EaJbCxJRnYuPtZSSnST2HREtu86JzQseGBwxZdGJxw1efBHvkm9ePs9nLwH9z2XzoN8qPUrnP9oMh0VFH5kFe57gYut19Z9mZ/1W84ze4imV2NYiWIlTlod2lvqg8Ymb+R+rMRAH2hSP8FCDnGaAu2MlUheFkobOLnjaQgKvKNFTe5Ugmuxmkj/3ZIMISdrBTnavNg1+YL8QkHzUb65SyNd11ndBDTMfnlfn5rgns4fvq7WcECrv7Mo+0//0fyMX3KeubnURvDWW/H2X5JnYrSdi5tP1fhG6M7Q0UmPlwZIrTrVC8kCcACNDf8k61koMcuFUErXJ4YuS1jsY7GsGajR5uNXgrcoPEDo3YuT258stk5LvXqO4Z+BUjR91fQKmH8xP/8Zi3mj4Eum3gH/1bYsjX76SGLDH0o3IenKJHak9HJaG8R73h7A8OvUlFHNg//ZnBAuyPcaW3hkc+RuselbJKIfzVvgICCM6BYkkGWpSQEj4zNExIMXSWJNTvW+4ppsUSRsmH+vGvlYDr47qsLKSI0cUo0m+PkZD5SYUYym3BjtWZmMvmGe+QGJAFGxpgnM+haVkuGZfo176vcgNNRV9QwJ3oaFDw68cRKWw6MnWxkJUzS0J2zbE7JF3GcMqSecnZeY8kBUhAyN6gP3AZd659xSb2QY/bhhbsP8X5L0sFYL+O/z14xQWEH6zCB709BziYXAIaT5DQ7cRciI65HF4552dT33Cueks+3jjq9ZZrvPYWVfHkMHIgDYIrecVMgoWRyGPIKmnC4O0gHZvKA088lAoBjcJJqkcwobGeCYNOQ5K9GPlch6LbEDJAIv+m3e6YJvFu8YtzepXuxTOJQa3461JC8GeGc7HApS4hIfsTcfKfW+PErgqFxzKKEPUr0vyQOrIFPA+lA+XBQxsNDeepK9+ejyFpWDtCKuf3m0jBNMcFgiyaQFGVqGDBrXS2CqpXYnuh9P7XpOCa3FmmTyYXYkNtibj3FN+bZ93AnldYoUpB6J7X8yp7sZPSQGDnV1fsM58WzLk0Fi7R4UtO9wVFA+l4PvxTbeqa9Bg7PRweZQIhulPa/GNt0jNh7unnaVc8K/lfGl5HulgVXOjvOGYDGyAdyGUVTIRBODjHjGFZHqBbgx4wHB+Zbo/mNsw21q4rOcfMg1CZwHQINTfcmdz8DLOfEr3rm3lXY/Ut8b0Q9vlINrTH7QzTvbtGQPgAKnKvW9Bq/Ett+D+snMA8pX4qMEjpQRGoasTWy6J7ZhKXQaCO3OjnOFhtnSntfcB14mNMxTwh+F114OnRtStdA7X09NOgcyN+iOJeKKcaF3/1OTBnl7Y6ki6md/0Tu6ZG6MXDKIFyHoQCdGqFBUhT/4QWLLw4g8K1qwtxzraD9N7n8DOHZOvgByh8i6a+TgWjioltyZ3PE3eXC1/5DfiUXEUGLrI5Gu63RvgRzjTwFnKfe/bm/7gqPtFCwHw2u/q0Q3Q8KsxrYAQ4MrTm04+N4C+kdREYykiz8mq8OWHGSJfPDD+KcPGin7gvtcnRfmN4EcgiMosU2R96+RB98Rm48MHPE0hOFizqB/2REgJgE11+SvQ45XvJT6cnDVeVhNCr6ZBlW8AJGe+JIsGeR2S4j6gndG8+fX5A0IhNdekdj2KJ0IDs0JZ1XgxSQ9vcSxTXfHN/+KqBwxEDjq72LgkAIyHg6/bwhe6B7+Rb8tOFSShEssRz78r+T2x40K36GPOieeldknvuW3kXXfrw0iVUiK8NaleNvPSvmMjXdQMjzTrg4c8ZfCCjENpSTc+hd4ZpI6sdz/Vmj1N4sNRUp7lmXcVaL7MSAvL3JTS/X8I7TmYvgTHLP5+FX66+3m415vPOoFY8lwELZpMiy/CBqbkAG9vPVE/6KHAWurvMkOuhgEqXfWDbyTLHxFBtvWXKLJwbxoEuki8+wd7af7D/m9b97PrA7l5O1NvKPNN3cpGbXXt4Tfv0oOreVMEbFmzsN204XVGFaNvJsSp5NYS+5Eko0Xhpcq9y0Pv38l/B731Kt8826D3lk6NxG8U+3jPgc+FkIGXAnS/8hSHxI9IMSC1M5nout/As7A0Xaid8aPUj0v0okR+nVz6W0zKA+sjG+6K/rRf8P+ro7zffPuIF+K9GePIx6yEmf7l0FaYiUMITxDBjSw+4BLM55DiW4KrbkQfoIYWBBY/L9i4yIkeEtn1462L9gcrdLAW1pqDxzc0fbFdGiKh1ZfAAmO7hGfEv3z6akWL3l54FA8HKp/OacmlVCXa/L59LqRH7v7pVGiOYycLBpeczEkHdmgSNMBTVITO6lotdBWRa6MvfkowTd7INxF1OuOv6WHrOAgCNyAJu1BZIYRdnV+0zH+NOhnEdB3g6vp7eek/TSFtjc0gGf6Dzwzry0cOgZH5ZpycaTr2twE2Z3rNu6n0Yd3Ty6NRfZq+mYKM69Tkz0QQZLb/ug+8LukM+h3sEGOpu8wG2Xu3Sp9KP9BXv9BSrgLIAYgwAs62s8YbYLUcEBuCP9qfGvR6k3HeaR6U360swd4wa+SLrfVnDemBXtS7+hkSrO99fNNzUcmtz+RhCQz/CGZ2m9vFNyd9uYjnR0XpKWGJYP5ggZcDrn0uiaFri/tfpEwHVjomfoflV0MZxvVIsnuP3vn/JTcq7Tt95y+SLl7ysUVXldjDb7E9icNOLJXAw13fKkSHJrsnPQ1fSICMuIimVcMF+VRSF+hu/gXPWShK8Mfpnb/H8RsCMOFlR+xebEc7gIH651xs36nJAYxCXKSOAY9j007KtLjXVMughcZdgfxwYvFlGzO8f0H++b/Qj9hhMhhVST6MiFPHnhLTeyAQ/sOvrdQWoKeSO38uyb1QVZCajN5LQruQfDoSSm5d02JbZZD6+Bn+hc8SGbu5JdP+iBQakrQ3nJcoXMVGuaSGq6myP0rtOQu+Mn21hN8tl+Q1AkCYrrIpCV3w1+l/tchzak7OFDTyf6DHrHI3LaRpyqJTYdbqMVdz4bWXESFJOj8wOK/Qh81H9J30M+V0Dpp4G3nxK9YjmJbhWpvmf5fv+7z4FU0zRlYqTuwZptnan4ITXwWfPscODf6T++cWzzTr8lxkxPO1OLbINipiW7gVQm+B/2Ed3fy7o6CCumW4MqzlOjHlPGG+XcD4uYdXJO/oSV2RjfciqV+Mg/Z2V7qtLEy8NrnzOp15LMVYlbP7YU+Aa4D+jG4h7yVubEai3TdQMmATqClekE8FtbTBP98ImlDNZ8vQhbY3aH/hF6p918FKcx9hIx0aSe64ebCeEqdKJaC8BvVJFk9QI13y/3569lFN95ukKFflKjVek766sSYQjlk5xACB9dXKmtUGAs5JlUmBC6RqLDcDq0le9XkDkoGR1bfRUr4o7x8kkR9fUE0UgIZAcN65JpQqFrgVOnPy8QYNZYPR3Ln39OCgKyyS2KNZ4rN1VEwkrAucwHpbQpqfFveRdQPZbwduh2q+ky7KsERWmElKpv0QgUGSYFyZ4HzzlYbjRQ2pz5KggXfdDL8kQPQTiW8nvr22qPB6w0JSSxE/cJk29wrQN/kCyasGTfOI10z6mtPC745hYJDaJidLre4oe2hC2UUaFp6K6B+0n5hqFvCNLm6XrY698qSCZvrL+R8h2oYpWR9shMSQPxjvYpFgm6+o/H45t4Weu87dOoKNL939k/z6h8QU+wtS8B5pHpeUKKf0MEUSGKJGs2O9oGWFPUxEdVI8yCFhi2aYsyzIltsZJ6VeQsPp2fTzw2nt4jQv3H25kpei3fDB5XIen04N4ds97RrpD2vZcKBe/o1xqRic8Vj4pk09hORq4t0iBn6ZLCcC+6ZeT1k4KA86KRU75ybaQqWUztpPz3+yf/AG8jqyUAj+bGFJ0z2lXpfoZPX66h8njfxVdO4gajNnI2DS/DOvSX/vmE6PNvzAmQioODyr6++7lH/siMgVI9ETCGtojOn2FtPChz+eF7NSpMGoKlIttJ8NBBc+PHgqvNAcetp1LfkgXfoY7ZAWfsP+V0eHxA9kzv+CodytJ5oOSIz+NaZ0p5X6RWp9UWoBhwFJTGNG8zyQVyrc9I55LqUf4R4d6TrusxdoyNoEDWaT+oqJzdOhxQp9vHP4pvu1R0hMg0FQzya3nT8yjKLYFS0kZGHrQ+TWtxITKoalglnNp4LeDRkUnbEAVQyuE8We68DMvSzxxWtCKtGN0GLGtHWRAZZdUNN0BtlyzTwrJDkG6Fk35KDOoKD9BIb9rmzNMgDKyMf/JCWkMsxObiaqw8DVRR+9ztS78uFhdoiZ76GrshuJkMXNjLkouH3LjNGDcs5VOj9su57yAGjmrdfD0tYyZYEUiiWzPKHxIamJctKlLQhm5V6l6V6/kHrzXVlhcWu/LpZ/5vK4OrYprvIEoAlm9+/6KHSc5Sk/hXK4Jro+ptxRbMwadajRKtVVh/eO13dDqxpOCGhTC+Mrv+pb97tvNhI9HbByGTs47tjG2/j6tISWx4S/fPF5sWQcxZm1+AaB984OaOxSghdhMTY5l/ZPFMF/0FYTRXOVJL2/Gtw5Vl7d+c0Ni/GV+eewyiCxHlJRmb/wdtbeEdrw8IHbI42MiXus7842r4o97+lz9pKcXVrZLi/A3Sib94dkMWAFgF5ITTMJXNCux9Tk7vLSihopECCzd0Jh/IvfFBoPFSLbweXI/gXQo6W3PF0dYdI6hoO+lB1RUW52S/Wp+Y2atKeOlnlqBJInMjeqN9eG8lp8soPBLKPF31Yk9OTpVH9rJNZCzi49EORVcurhwRym6ESq//FQ/PgLv7PvSROvxVW3ud6RtXwqtW9U4jze4o8bg4rVdRQtctwS/1z7w6p3yexb2TkDUGMDjho8aPBM8rCxyg0XMVuVtN7ZUUbbnAzPoYTjarK+VrfSO0QscfJ+BgdNgJ32bsd2GXH7NIzOKzN69LsIuODwVHEGlyaYGN8MDgsky6S3Go2njUBg8Pyu/XiB0KsFRgcVgaew8+SWwZHMRMFVvxgcBQ3VvxgcJQyVvxgcJQyVvxgcJQyVvxgcBQ1VvxgcJQ8IcRB8sKKHwwOa4PIwoofDI6ixoofDI5SxoofDI5SxoofDI5SxoofDI5SxoofDI6iRoofbo1nxQ8Gh/VZ6iP7rPjB4LA2VvxgcJSywuIHec7vzBssV9pnVoUOOaJf7keBYznvfP19gFOCXLIb9z3LKdZrS8LOzsASbfP90WgkDcdk76wbpL7XE91/KlfBiH6ACQnG845SPc8r+7wCHxzT3nKs0HAQ6W2iX9PXxlTCH8CR85eOZHCUdUGn3IgmXcUJ+Q+lQtxD+LP78NalFojARwLHujlOXX97Zs2Pigyw8M270/wkLGArvvn+vBXyK8LCfeAVnqlXFHu6FvDR/+pRDI5Krumsh9D4r5N3yW3kkeOK8WgS1HIGOBI06UqAQFt7cjEX4nVpKs5Z86McgxhEyUjtei7+6QMkTjUvATjcU68A/0HXKa/Q8c0PHP4EWWZDDoHrSvW8gNPPWIGNYvMxzvbTtZFZYXfUwoGm3UXJwJ/8EJyE+U94662ABZr3FEFk3lN47RdyPrn1VhxYTj/S4NKCWmWCydF+GpABXTm46ly6BYCAKABwODvOrxQOIKPp6BfhgPDB4Kp/L1hmmUQ6cEjlL0PIBCnRDeAYCAcbLs0jw+AjuJz4DLon9S6mPwE91J3Q4kdFyS3VGcldz5s3Sv1v0I5e6Q/xL/w1dUKDK04ptgA3bGeaoxIbf4HRzD2PFd0nug40B3l03KQrzbuBywGPAv4GdqDFD6+zgsop1qlytp9mXnbMMf5UjqyEWVkTgnYBzwFtH3rvsr3QPfClIGAzMgW+HcIcSJ+8PfVE7HwzuLAnOCrYWanJwwJq7jla9CfKWPmMnIbs0bMPSGScpj4NsQbciZB11Da+AjhSus+ARnW0n55pJ5oGJ7f/ubIIpSMFgaPSRfvh2xsW/pqeADQzvKCZqRgCFMx7wm6gh+BP5Lkipj1rmbfX1nNAY+u+nYjQ0pbcRl7OTuTsxMltQ9bHyvly6HbRDbfBFYeIEJSDjvGngdqA7WRt0AoFB21dkLGVXgBoYPAQEMsgHpl9CaAAKU/GpQFDLvJc0tDAilPMTgLgsLcsUWr1jJGaeg6j0yfL8uEGE4ElQx8WcWXO/ICrD90d/Hnj0S8CGXD1QZyWXyPJtFBGz+7FRQCVaiaDeiBob71YYvxYXo84cmhdHgfAd6VnO8qyFZysvkbLW/C0RI93pmMKGCiGvHYqD47JxWoeon9+4XZo4yGjD3AmFHwWjgYblZF4FtEIF8GqH7JcmqLxslKUD3DdNGBDS0AXhPcQXwbi28xXH+gJHP4EEJNJd8s3aEtwSIXbIZ2x9DHmxy3k1dCowqDZcmzz/SCJRiTrGYk6B8iI4TkyJLcFC56alT+J4uDVM54ZtsDVN8d1qjRpflvMitW1YHseBOT5sQWVU2DCfeB3HSYHZmlwVoHDn4SdQSTBC3hNbH98L/zcqIEDK0HSbs7OMhki/wtWENdp8WMwZtMKFIhH155mMsJ6FmrmAxqSBp3UrudLi8qMbDR7HXgPTsK8JziSvKexAKPQ0rQEIutpSIaYvD1hBzgabHR2nE+y3/bT4QVfAaFw/xSkpD5BS1gtQ/QbApAOB46+X9nvsZr5AZcYGr5QzQEfVJ8CH9DS3pk3wHvYMqQPp81j+Rie0nksJQMY7f3HRGh7EMj0VewbgR44SdgZPgK40ECzFyW70VHnIIOuYHqRtJQPoFXU4PJiwyulM1vLmR+WD30y80Hzl9jG24f8ClpmhehQYXXkNJqbFNa7Src3nBV8ZM/Lcwz31nH+fgrH1ls5q9J4XjmEwjFkrayoNrS67aVYlhHWcxYqDkD9lSP94p/eDw1GZ5NUem6FjJqT2NKIJCos1o0yOMgwrN7kZGDWyn8AN/yCl2ihbOhaWYluarrtBZwzbUta9SrMb+0tx2akiWCVjha2Ex3lhzBRKR+FHPjm3VkYgKw/23zMfp6tkMERwQ+egwzPTrqycMieqhO84ZJ9L36oKk7qI/vRjbdBG8BLaDgoUywHXMBF09aicqQwfylmsD/vmkxTCdfk80HD5hVMIYjklT3ge2Fn+DrgSe5/XfcZAYhNsAVoM+c1cJ7g5xLdf6a70T1dHefRHCdZK/8xMnUOvOFScCFkso+zs9B/4K1LiXdRqjATwufW1BgpfkDMRoJfb8gL8oYnII7oDxt8rlh+W8xASELjEeD888EnWbolOGwmjsAXQQijgyZ5nCnhD8z+A8tBm3tJ3m7UY0G2UrOaR40eqVFMvmenCRqdq9Q0QaJFhADJXzI7wBG8B0OGTMdprUE0Pe2FJqvQ47Ply/7X84oT1JFocqj8jJE+AhflzmqDI1vWRqm7ytKjz1Ok1VXz/nBAsTknAGmJ7uSu52r5fO4RhaNWpmmcZfGDWZ0J0hH5key2FwbHXhQ/mDE4ihY/mDE4ssUPyG9ZqzM4rM3j1BxsTQcGRzGD4AIhhrU9g8PaQJyyNT8YHNbG1vxgcJT85az4weAoYaz4weAoZaBMfS7GB4OjiDntrPjB4ChurPjB4ChlrPjB4ChlrPjB4ChqrPjB4Ch5OVjxg8FRwiCysJF9BkdRswvYy4ofDI5i5mLFDwZHCWPFDwZHKRvjxY//F2AAQYXA4AmvyD8AAAAASUVORK5CYII=";
    $scope.imageArquivo = imagem_padrão;
    $scope.objImagem = {};
    $scope.tituloModal = "";
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.implementos = [];
    $scope.selecionado = [];
    $scope.implementosSelecionados = [];

    consultar();

    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("tiposEquipamento.xlsx",{headers:true}) FROM ?', [$scope.tiposEquipamento]);
    };

    function consultar() {
        addLoader();
        tipoEquipamentoService.consultar().then(function (response) {
            $scope.tiposEquipamento = response.data;
            removeLoader()
        });
    }

    $scope.loadImplementos = function () {
        implementoService.consultar().then(function (response) {
            $scope.implementos = response.data;
            if ($scope.tipoEquipamento.Codigo != undefined) {
                for (var i = 0; i < $scope.tipoEquipamento.Implementos.length; i++) {
                    $scope.selecionado.push($scope.tipoEquipamento.Implementos[i].Codigo);
                }
            }
        });
    }

    $scope.verificaChecados = function (codigo) {
        return $scope.selecionado.indexOf(codigo) > -1;
    }

    $scope.toggleSelection = function toggleSelection(selecionado) {
        var idx = $scope.selecionado.indexOf(selecionado);
        if (idx > -1) {
            $scope.selecionado.splice(idx, 1);
        }
        else {
            $scope.selecionado.push(selecionado);
        }
    };


    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
        $scope.imageArquivo = imagem_padrão;
        $scope.implementosSelecionados = [];
        $scope.selecionado = [];
    };

    $scope.cancel_imagem = function () {
        $(":file").filestyle('clear');
        $scope.objImagem = {};
        $scope.imageArquivo = imagem_padrão;
    };

    $scope.onFilesSelected = function (data) {
        var file = data[0];
        $scope.objImagem = file;
        var reader = new FileReader();
        reader.onload = function (evt) {
            $scope.$apply(function () {
                var v = evt.target.result;
                if (v.startsWith('data:')) {
                    var ext = v.split(';')[0].split('/')[1];
                    if (['png', 'jpeg', 'jpg', 'bmp'].indexOf(ext) >= 0) {
                        $scope.tipoEquipamento.Extensao = ext;
                        $timeout(function () { $scope.imageArquivo = v; });
                    }
                }
            });
        };
        if (file)
            reader.readAsDataURL(file);
    };

    $scope.abrirModal = function () {
        $scope.objImagem = {};
        $scope.tipoEquipamento = { Imagem: "", Extensao: "" };
        $scope.tituloModal = "Tipo de Equipamento - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalTipoEquipamento',
            scope: $scope
        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.tipoEquipamento = { Imagem: "", Extensao: "" };
        });
    }

    $scope.montaObjeto = function () {
        $scope.implementosSelecionados = []
        for (var i = 0; i < $scope.selecionado.length; i++) {
            var impl = { Codigo: $scope.selecionado[i] };
            var TipoEquipamentoImplemento = {Implemento: impl};
            $scope.implementosSelecionados.push(TipoEquipamentoImplemento);
        }
        $scope.tipoEquipamento.Implementos = $scope.implementosSelecionados;
    }

    $scope.salvar = function () {
        addLoader();
        $scope.montaObjeto();
        if ($scope.tipoEquipamento.Codigo == undefined) {
            tipoEquipamentoService.cadastrar($scope.tipoEquipamento, $scope.objImagem).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.tiposEquipamento, response.data);
                    $scope.objImagem = {};
                    $scope.tipoEquipamento = { Imagem: "", Extensao: "" };
                    $scope.implementosSelecionados = [];
                    $scope.selecionado = [];
                    $scope.cancel();

                }
            }, function (error) {

            });
        } else {

            tipoEquipamentoService.atualizar($scope.tipoEquipamento, $scope.objImagem).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.tiposEquipamento, response.data);
                    $scope.objImagem = {};
                    $scope.tipoEquipamento = { Imagem: "", Extensao: "" };
                    $scope.cancel();
                }

            }, function (error) {

            });
        }

    }

    $scope.editar = function (data) {
        addLoader();
        tipoEquipamentoService.editar(data).then(function (response) {
            removeLoader();
            if (response.data.Imagem != "") {
                $scope.imageArquivo = response.data.Imagem;
            }
            response.data.Imagem = "";
            $scope.tipoEquipamento = response.data;
            $scope.tituloModal = "Tipo Equipamento - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalTipoEquipamento',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.objImagem = {};
                $scope.tipoEquipamento = { Imagem: "", Extensao: "" };
                $scope.implementosSelecionados = [];
                $scope.selecionado = [];
            });
        });
    }

    $scope.deletar = function (data) {
        swal({
            title: "Atenção",
            text: "Você tem certeza que gostaria de remover este registro?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
            closeOnConfirm: true,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                addLoader();
                tipoEquipamentoService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.tiposEquipamento, response.data);
                    }
                });

            }
        });
    }


}]);