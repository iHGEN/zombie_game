using System.Linq;
using UnityEngine;

public class Sentry_Gun : MonoBehaviour
{
    public RaycastHit raycastHit;
    [SerializeField] int range;
    [SerializeField] float firerate;
    [SerializeField] GameObject _bullet;
    [SerializeField] GameObject _fire_point;
    [SerializeField] int damge;
    [SerializeField] Zombie_wave zombie_Wave;
    [SerializeField] Money _money;
    [SerializeField] float _distance_to_fire;
    [SerializeField] AudioClip audioClip;
    public bool _is_finsh = true;
    [SerializeField] ParticleSystem _particleSystem;
    public int sentry_time;
    AudioSource audioSource;
    int get_zombie_number;
    bool _is_zombie_near_distance_found;
    float nexttimefire;
    void Start()
    {
        _is_finsh = true;
        audioSource = GetComponent<AudioSource>();
    }
    void check_nearest_zombie(GameObject[] zombies)
    {
        for (int i = 0; i < zombies.Length; i++)
        {
            if (zombies[i] != null)
            {
                if (zombies[i].activeInHierarchy && Vector3.Distance(this.transform.position, zombies[i].transform.position) < _distance_to_fire)
                {
                    _is_zombie_near_distance_found = true;
                    get_zombie_number = i;
                    break;
                }
                else
                {
                    _is_zombie_near_distance_found = false;
                }
            }
        }
    }
    void _fire()
    {
        if (nexttimefire > firerate)
        {
            if (Physics.Raycast(this.transform.position, this.transform.forward, out raycastHit, range))
            {
                if (!_particleSystem.isPlaying)
                {
                    _particleSystem.Play();
                }
                GameObject bullet = Instantiate(_bullet, _fire_point.transform.position, this.transform.rotation);
                if (raycastHit.transform.gameObject.tag == "zm" && zombie_Wave._zombie_id_number.Count != 0)
                {
                    int zombie_id = zombie_Wave._zombie_id_number.First(x => x.Value == raycastHit.transform.gameObject.GetInstanceID()).Key;
                    if (!zombie_Wave.zombie_Healths[zombie_id]._is_die)
                    {
                        zombie_Wave.zombie_Healths[zombie_id].Helath -= damge;
                        _money.add_money(_money.zombie_hit_money);
                    }
                }
                bullet.GetComponent<bullet_forword>().sentry = this;
                bullet.GetComponent<bullet_forword>()._is_Sentry_Gun = true;
                bullet.GetComponent<BoxCollider>().isTrigger = true;
                bullte_forword(bullet, raycastHit.point);
                Destroy(bullet, 2f);
            }
            audioSource.PlayOneShot(audioClip, 0.7f);
            nexttimefire = 0;
        }
    }
    public void Custom_lockat(GameObject target)
    {
        Vector3 dirction = target.transform.position - gameObject.transform.position;
        Quaternion lockat = Quaternion.LookRotation(dirction);
        Vector3 rot = lockat.eulerAngles;
        gameObject.transform.rotation = Quaternion.Euler(rot.x - 0.2f, rot.y, 0);
    }
    void bullte_forword(GameObject gameObject, Vector3 endlocation)
    {
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, endlocation, 0.1f);
    }
    // Update is called once per frame
    void Update()
    {
        nexttimefire += Time.deltaTime;
        if (_is_finsh) { _particleSystem.Stop(); return; }
        if (zombie_Wave.is_setup_finish)
        {
            check_nearest_zombie(zombie_Wave._Zombie_charcter);
            if (_is_zombie_near_distance_found)
            {
                Custom_lockat(zombie_Wave._Zombie_charcter[get_zombie_number]);
            }
            else
            {
                return;
            }
        }
            _fire();
    }
}
